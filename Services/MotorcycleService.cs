using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories;
using Services.DTOs;
using Services.Interfaces;

namespace Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepo;
    private readonly IMotorcycleTypeRepository _typeRepo;

    public MotorcycleService(
        IMotorcycleRepository motorcycleRepo,
        IMotorcycleTypeRepository typeRepo)
    {
        _motorcycleRepo = motorcycleRepo;
        _typeRepo = typeRepo;
    }

    public async Task<PaginatedResult<MotorcycleDto>> GetPaginatedAsync(
        string? search,
        MotorcycleStatus? status,
        decimal? minPrice,
        decimal? maxPrice,
        int page = 1,
        int pageSize = 10)
    {
        var (items, totalCount) = await _motorcycleRepo.SearchAsync(
            search, status, minPrice, maxPrice, page, pageSize);

        return new PaginatedResult<MotorcycleDto>
        {
            Items = items.Select(MapToDto).ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalCount
        };
    }

    public async Task<MotorcycleDetailDto?> GetDetailAsync(int id)
    {
        var motorcycle = await _motorcycleRepo.GetByIdWithDetailsAsync(id);
        if (motorcycle == null) return null;

        return MapToDetailDto(motorcycle);
    }

    public async Task<MotorcycleDto?> GetByIdAsync(int id)
    {
        var motorcycle = await _motorcycleRepo.GetByIdWithDetailsAsync(id);
        return motorcycle == null ? null : MapToDto(motorcycle);
    }

    public async Task<List<MotorcycleDto>> GetAvailableAsync()
    {
        var motorcycles = await _motorcycleRepo.GetAvailableAsync();
        return motorcycles.Select(MapToDto).ToList();
    }

    public async Task<MotorcycleDto> CreateAsync(CreateMotorcycleRequest request)
    {
        if (await _motorcycleRepo.ExistsByLicensePlateAsync(request.LicensePlate))
            throw new InvalidOperationException("License plate already exists in the system.");

        var registrationNo = string.IsNullOrWhiteSpace(request.RegistrationNo)
            ? null
            : request.RegistrationNo.Trim();
        if (registrationNo != null && await _motorcycleRepo.ExistsByRegistrationNoAsync(registrationNo))
            throw new InvalidOperationException("Registration number already exists in the system.");

        var type = await _typeRepo.GetByIdAsync(request.VehicleTypeId);
        if (type == null || !type.IsActive)
            throw new InvalidOperationException("Vehicle type not found or inactive.");

        var motorcycle = new Motorcycle
        {
            LicensePlate = request.LicensePlate.Trim().ToUpper(),
            Brand = request.Brand.Trim(),
            Model = request.Model.Trim(),
            VehicleTypeId = request.VehicleTypeId,
            Color = request.Color.Trim(),
            Mileage = request.Mileage,
            RegistrationNo = registrationNo,
            Status = MotorcycleStatus.Available,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _motorcycleRepo.AddAsync(motorcycle);

        motorcycle.VehicleType = type;
        return MapToDto(motorcycle);
    }

    public async Task<MotorcycleDto> UpdateAsync(int id, UpdateMotorcycleRequest request, string userRole)
    {
        var motorcycle = await _motorcycleRepo.GetByIdWithDetailsAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");

        bool isAdmin = userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);

        if (isAdmin)
        {
            if (request.LicensePlate != null)
            {
                var plate = request.LicensePlate.Trim().ToUpper();
                if (await _motorcycleRepo.ExistsByLicensePlateAsync(plate, id))
                    throw new InvalidOperationException("License plate already exists in the system.");
                motorcycle.LicensePlate = plate;
            }
            if (request.Brand != null) motorcycle.Brand = request.Brand.Trim();
            if (request.Model != null) motorcycle.Model = request.Model.Trim();
            if (request.VehicleTypeId.HasValue)
            {
                var type = await _typeRepo.GetByIdAsync(request.VehicleTypeId.Value);
                if (type == null || !type.IsActive)
                    throw new InvalidOperationException("Vehicle type not found or inactive.");
                motorcycle.VehicleTypeId = request.VehicleTypeId.Value;
            }
            if (request.RegistrationNo != null)
            {
                var registrationNo = string.IsNullOrWhiteSpace(request.RegistrationNo)
                    ? null
                    : request.RegistrationNo.Trim();
                if (registrationNo != null && await _motorcycleRepo.ExistsByRegistrationNoAsync(registrationNo, id))
                    throw new InvalidOperationException("Registration number already exists in the system.");
                motorcycle.RegistrationNo = registrationNo;
            }
        }
        else
        {
            if (request.LicensePlate != null || request.Brand != null || request.Model != null ||
                request.VehicleTypeId != null)
                throw new UnauthorizedAccessException("Staff can only edit Color and Mileage.");
        }

        if (request.Color != null) motorcycle.Color = request.Color.Trim();
        if (request.Mileage.HasValue) motorcycle.Mileage = request.Mileage.Value;

        motorcycle.UpdatedAt = DateTime.UtcNow;
        _motorcycleRepo.Update(motorcycle);

        return MapToDto(motorcycle);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var motorcycle = await _motorcycleRepo.GetByIdAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");

        if (await _motorcycleRepo.HasActiveRentalsAsync(id))
            throw new InvalidOperationException("Cannot deactivate motorcycle with active or reserved rentals.");

        motorcycle.IsActive = false;
        motorcycle.UpdatedAt = DateTime.UtcNow;
        _motorcycleRepo.Update(motorcycle);
    }

    public async Task UpdateStatusAsync(int id, MotorcycleStatus newStatus)
    {
        var motorcycle = await _motorcycleRepo.GetByIdAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");

        motorcycle.Status = newStatus;
        motorcycle.UpdatedAt = DateTime.UtcNow;
        _motorcycleRepo.Update(motorcycle);
    }

    public async Task UpdateMileageAsync(int id, int newMileage)
    {
        var motorcycle = await _motorcycleRepo.GetByIdAsync(id);
        if (motorcycle == null)
            throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");

        motorcycle.Mileage = newMileage;
        motorcycle.UpdatedAt = DateTime.UtcNow;
        _motorcycleRepo.Update(motorcycle);
    }

    private static MotorcycleDto MapToDto(Motorcycle m) => new()
    {
        Id = m.Id,
        LicensePlate = m.LicensePlate,
        Brand = m.Brand,
        Model = m.Model,
        VehicleTypeId = m.VehicleTypeId,
        VehicleTypeName = m.VehicleType?.Name ?? "",
        Status = m.Status,
        DailyRate = m.VehicleType?.DefaultDailyRate ?? 0,
        Color = m.Color,
        Mileage = m.Mileage,
        RegistrationNo = m.RegistrationNo,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt
    };

    private static MotorcycleDetailDto MapToDetailDto(Motorcycle m) => new()
    {
        Id = m.Id,
        LicensePlate = m.LicensePlate,
        Brand = m.Brand,
        Model = m.Model,
        VehicleTypeId = m.VehicleTypeId,
        VehicleTypeName = m.VehicleType?.Name ?? "",
        Status = m.Status,
        DailyRate = m.VehicleType?.DefaultDailyRate ?? 0,
        Color = m.Color,
        Mileage = m.Mileage,
        RegistrationNo = m.RegistrationNo,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt,
        RecentRentals = m.RentalContracts.Select(r => new RentalHistoryItem
        {
            Id = r.Id,
            CustomerName = r.Customer?.FullName ?? "",
            RentalDate = r.RentalDate,
            ExpectedReturnDate = r.ExpectedReturnDate,
            ActualReturnDate = r.ActualReturnDate,
            Status = r.Status,
            TotalAmount = r.TotalAmount
        }).ToList()
    };
}
