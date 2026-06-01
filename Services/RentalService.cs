using Repositories;
using BusinessObjects;
using Services.Interfaces;
using Services.DTOs;

namespace Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ICustomerRepository _customerRepository;

    public RentalService(
        IRentalRepository rentalRepository,
        IMotorcycleRepository motorcycleRepository,
        ICustomerRepository customerRepository)
    {
        _rentalRepository = rentalRepository;
        _motorcycleRepository = motorcycleRepository;
        _customerRepository = customerRepository;
    }

    public async Task<RentalDto?> GetByIdAsync(int id)
    {
        var rental = await _rentalRepository.GetByIdWithDetailsAsync(id);
        return rental == null ? null : MapToDto(rental);
    }

    public async Task<IEnumerable<RentalDto>> GetAllAsync()
    {
        var rentals = await _rentalRepository.GetAllAsync();
        return rentals.Select(MapToDto);
    }

    public async Task<PaginatedResult<RentalDto>> GetPaginatedAsync(
        int page, int pageSize, RentalStatus? status,
        int? customerId, int? motorcycleId, DateTime? dateFrom, DateTime? dateTo)
    {
        var rentals = await _rentalRepository.SearchAsync(status, customerId, motorcycleId, dateFrom, dateTo, page, pageSize);
        var totalCount = await _rentalRepository.GetTotalCountAsync(status, customerId, motorcycleId, dateFrom, dateTo);

        return PaginatedResult<RentalDto>.Create(
            rentals.Select(MapToDto),
            page, pageSize, totalCount);
    }

    public async Task<RentalDto> CreateAsync(CreateRentalDto dto, string createdBy)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(dto.MotorcycleId);
        if (motorcycle == null || motorcycle.Status != MotorcycleStatus.Available)
            throw new InvalidOperationException("Motorcycle is not available for rental");

        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        if (dto.ExpectedReturnDate < dto.RentalDate)
            throw new InvalidOperationException("Expected return date must be after rental date");

        var days = (int)Math.Ceiling((dto.ExpectedReturnDate - dto.RentalDate).TotalDays);
        var totalAmount = days * motorcycle.DailyRate;

        var rental = new RentalContract
        {
            CustomerId = dto.CustomerId,
            MotorcycleId = dto.MotorcycleId,
            RentalDate = dto.RentalDate,
            ExpectedReturnDate = dto.ExpectedReturnDate,
            DailyRate = motorcycle.DailyRate,
            TotalAmount = totalAmount,
            DepositAmount = dto.DepositAmount,
            FinalAmount = 0,
            Status = RentalStatus.Pending,
            Notes = dto.Notes,
            CreatedBy = createdBy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        motorcycle.Status = MotorcycleStatus.Reserved;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await _rentalRepository.AddAsync(rental);
        await _motorcycleRepository.UpdateAsync(motorcycle);

        rental.Customer = customer;
        rental.Motorcycle = motorcycle;
        return MapToDto(rental);
    }

    public async Task<RentalDto?> ActivateAsync(int id, ActivateRentalDto dto)
    {
        var rental = await _rentalRepository.GetByIdWithDetailsAsync(id);
        if (rental == null || rental.Status != RentalStatus.Pending) return null;

        var motorcycle = await _motorcycleRepository.GetByIdAsync(rental.MotorcycleId);
        if (motorcycle == null || dto.StartMileage < motorcycle.Mileage)
            throw new InvalidOperationException("Start mileage must be greater than or equal to current mileage");

        rental.Status = RentalStatus.Active;
        rental.StartMileage = dto.StartMileage;
        rental.UpdatedAt = DateTime.UtcNow;

        motorcycle.Status = MotorcycleStatus.Rented;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await _rentalRepository.UpdateAsync(rental);
        await _motorcycleRepository.UpdateAsync(motorcycle);

        return MapToDto(rental);
    }

    public async Task<RentalDto?> CompleteAsync(int id, CompleteRentalDto dto)
    {
        var rental = await _rentalRepository.GetByIdWithDetailsAsync(id);
        if (rental == null || rental.Status != RentalStatus.Active) return null;

        if (dto.EndMileage < rental.StartMileage)
            throw new InvalidOperationException("End mileage must be greater than or equal to start mileage");

        var motorcycle = await _motorcycleRepository.GetByIdAsync(rental.MotorcycleId);
        if (motorcycle == null) return null;

        var actualDays = (int)Math.Ceiling((dto.ActualReturnDate - rental.RentalDate).TotalDays);
        var expectedDays = (int)Math.Ceiling((rental.ExpectedReturnDate - rental.RentalDate).TotalDays);

        decimal finalAmount = actualDays > expectedDays
            ? rental.TotalAmount + ((actualDays - expectedDays) * rental.DailyRate)
            : rental.TotalAmount;

        rental.Status = RentalStatus.Completed;
        rental.ActualReturnDate = dto.ActualReturnDate;
        rental.EndMileage = dto.EndMileage;
        rental.FinalAmount = finalAmount;
        rental.UpdatedAt = DateTime.UtcNow;

        motorcycle.Status = MotorcycleStatus.Available;
        motorcycle.Mileage = dto.EndMileage;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await _rentalRepository.UpdateAsync(rental);
        await _motorcycleRepository.UpdateAsync(motorcycle);

        return MapToDto(rental);
    }

    public async Task<RentalDto?> CancelAsync(int id, string? reason)
    {
        var rental = await _rentalRepository.GetByIdWithDetailsAsync(id);
        if (rental == null || rental.Status != RentalStatus.Pending) return null;

        var motorcycle = await _motorcycleRepository.GetByIdAsync(rental.MotorcycleId);
        if (motorcycle == null) return null;

        rental.Status = RentalStatus.Cancelled;
        rental.Notes = string.IsNullOrEmpty(rental.Notes) ? reason : $"{rental.Notes}; Cancellation: {reason}";
        rental.UpdatedAt = DateTime.UtcNow;

        motorcycle.Status = MotorcycleStatus.Available;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await _rentalRepository.UpdateAsync(rental);
        await _motorcycleRepository.UpdateAsync(motorcycle);

        return MapToDto(rental);
    }

    private static RentalDto MapToDto(RentalContract r) => new()
    {
        Id = r.Id,
        CustomerId = r.CustomerId,
        CustomerName = r.Customer?.FullName,
        MotorcycleId = r.MotorcycleId,
        MotorcyclePlate = r.Motorcycle?.LicensePlate,
        RentalDate = r.RentalDate,
        ExpectedReturnDate = r.ExpectedReturnDate,
        ActualReturnDate = r.ActualReturnDate,
        DailyRate = r.DailyRate,
        TotalAmount = r.TotalAmount,
        DepositAmount = r.DepositAmount,
        FinalAmount = r.FinalAmount,
        Status = r.Status,
        StartMileage = r.StartMileage,
        EndMileage = r.EndMileage,
        Notes = r.Notes,
        CreatedBy = r.CreatedBy,
        CreatedAt = r.CreatedAt,
        UpdatedAt = r.UpdatedAt
    };
}
