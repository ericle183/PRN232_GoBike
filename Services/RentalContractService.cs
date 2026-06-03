using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories;
using Services.Interfaces;

namespace Services;

public class RentalContractService : IRentalContractService
{
    private readonly IRentalContractRepository rentalContractRepository;
    private readonly ICustomerRepository customerRepository;
    private readonly IMotorcycleRepository motorcycleRepository;

    public RentalContractService(
        IRentalContractRepository rentalContractRepository,
        ICustomerRepository customerRepository,
        IMotorcycleRepository motorcycleRepository)
    {
        this.rentalContractRepository = rentalContractRepository;
        this.customerRepository = customerRepository;
        this.motorcycleRepository = motorcycleRepository;
    }

    public Task<List<RentalContract>> GetAllAsync()
        => rentalContractRepository.GetAllAsync();

    public Task<RentalContract?> GetByIdAsync(int id)
        => rentalContractRepository.GetByIdAsync(id);

    public async Task CreateAsync(RentalContract rentalContract)
    {
        await ValidateRentalAsync(rentalContract);

        rentalContract.TotalAmount = CalculateTotalAmount(rentalContract.RentalDate, rentalContract.ExpectedReturnDate, rentalContract.DailyRate);
        rentalContract.Status = RentalStatus.Pending;
        rentalContract.IsActive = true;
        rentalContract.CreatedAt = DateTime.UtcNow;

        var motorcycle = await motorcycleRepository.GetByIdAsync(rentalContract.MotorcycleId)
            ?? throw new InvalidOperationException($"Motorcycle with ID {rentalContract.MotorcycleId} not found");

        if (motorcycle.Status != MotorcycleStatus.Available)
        {
            throw new InvalidOperationException("Motorcycle must be available to create rental");
        }

        motorcycle.Status = MotorcycleStatus.Reserved;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await rentalContractRepository.AddAsync(rentalContract);
        motorcycleRepository.Update(motorcycle);
    }

    public async Task UpdateAsync(RentalContract rentalContract)
    {
        await ValidateRentalAsync(rentalContract);

        rentalContract.TotalAmount = CalculateTotalAmount(rentalContract.RentalDate, rentalContract.ExpectedReturnDate, rentalContract.DailyRate);
        rentalContract.UpdatedAt = DateTime.UtcNow;
        rentalContractRepository.Update(rentalContract);
    }

    public async Task DeleteAsync(int id)
    {
        var rental = await rentalContractRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"RentalContract with ID {id} not found");

        rental.IsActive = false;
        rental.UpdatedAt = DateTime.UtcNow;
        rentalContractRepository.Update(rental);
    }

    public async Task ActivateAsync(int id, int startMileage)
    {
        var rental = await rentalContractRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"RentalContract with ID {id} not found");

        if (rental.Status != RentalStatus.Pending)
        {
            throw new InvalidOperationException("Only pending rentals can be activated");
        }

        var motorcycle = await motorcycleRepository.GetByIdAsync(rental.MotorcycleId)
            ?? throw new InvalidOperationException($"Motorcycle with ID {rental.MotorcycleId} not found");

        if (startMileage < 0 || startMileage < motorcycle.Mileage)
        {
            throw new InvalidOperationException("StartMileage must be greater than or equal to current motorcycle mileage");
        }

        rental.StartMileage = startMileage;
        rental.Status = RentalStatus.Active;
        rental.UpdatedAt = DateTime.UtcNow;
        motorcycle.Status = MotorcycleStatus.Rented;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        rentalContractRepository.Update(rental);
        motorcycleRepository.Update(motorcycle);
    }

    public async Task CompleteAsync(int id, DateTime actualReturnDate, int endMileage)
    {
        var rental = await rentalContractRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"RentalContract with ID {id} not found");

        if (rental.Status != RentalStatus.Active)
        {
            throw new InvalidOperationException("Only active rentals can be completed");
        }

        if (rental.StartMileage is null)
        {
            throw new InvalidOperationException("StartMileage is required before completing rental");
        }

        if (endMileage < rental.StartMileage.Value)
        {
            throw new InvalidOperationException("EndMileage must be greater than or equal to StartMileage");
        }

        var motorcycle = await motorcycleRepository.GetByIdAsync(rental.MotorcycleId)
            ?? throw new InvalidOperationException($"Motorcycle with ID {rental.MotorcycleId} not found");

        rental.ActualReturnDate = actualReturnDate;
        rental.EndMileage = endMileage;
        rental.FinalAmount = CalculateFinalAmount(rental.RentalDate, rental.ExpectedReturnDate, actualReturnDate, rental.DailyRate);
        rental.Status = RentalStatus.Completed;
        rental.UpdatedAt = DateTime.UtcNow;

        motorcycle.Status = MotorcycleStatus.Available;
        motorcycle.Mileage = endMileage;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        rentalContractRepository.Update(rental);
        motorcycleRepository.Update(motorcycle);
    }

    public async Task CancelAsync(int id)
    {
        var rental = await rentalContractRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"RentalContract with ID {id} not found");

        if (rental.Status != RentalStatus.Pending)
        {
            throw new InvalidOperationException("Only pending rentals can be cancelled");
        }

        var motorcycle = await motorcycleRepository.GetByIdAsync(rental.MotorcycleId)
            ?? throw new InvalidOperationException($"Motorcycle with ID {rental.MotorcycleId} not found");

        rental.Status = RentalStatus.Cancelled;
        rental.UpdatedAt = DateTime.UtcNow;
        motorcycle.Status = MotorcycleStatus.Available;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        rentalContractRepository.Update(rental);
        motorcycleRepository.Update(motorcycle);
    }

    private async Task ValidateRentalAsync(RentalContract rentalContract)
    {
        var customer = await customerRepository.GetByIdAsync(rentalContract.CustomerId)
            ?? throw new InvalidOperationException($"Customer with ID {rentalContract.CustomerId} not found");
        _ = customer;

        var motorcycle = await motorcycleRepository.GetByIdAsync(rentalContract.MotorcycleId)
            ?? throw new InvalidOperationException($"Motorcycle with ID {rentalContract.MotorcycleId} not found");
        _ = motorcycle;

        if (rentalContract.ExpectedReturnDate < rentalContract.RentalDate)
        {
            throw new InvalidOperationException("ExpectedReturnDate must be greater than or equal to RentalDate");
        }

        if (rentalContract.DailyRate <= 0)
        {
            throw new InvalidOperationException("DailyRate must be greater than zero");
        }
    }

    private static decimal CalculateTotalAmount(DateTime rentalDate, DateTime expectedReturnDate, decimal dailyRate)
    {
        var days = Math.Max(1, (int)Math.Ceiling((expectedReturnDate.Date - rentalDate.Date).TotalDays));
        return days * dailyRate;
    }

    private static decimal CalculateFinalAmount(DateTime rentalDate, DateTime expectedReturnDate, DateTime actualReturnDate, decimal dailyRate)
    {
        var totalAmount = CalculateTotalAmount(rentalDate, expectedReturnDate, dailyRate);
        if (actualReturnDate.Date <= expectedReturnDate.Date)
        {
            return totalAmount;
        }

        var extraDays = (int)Math.Ceiling((actualReturnDate.Date - expectedReturnDate.Date).TotalDays);
        return totalAmount + (extraDays * dailyRate);
    }
}
