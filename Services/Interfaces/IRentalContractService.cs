using BusinessObjects.Entities;

namespace Services.Interfaces;

public interface IRentalContractService
{
    Task<List<RentalContract>> GetAllAsync();
    Task<RentalContract?> GetByIdAsync(int id);
    Task CreateAsync(RentalContract rentalContract);
    Task UpdateAsync(RentalContract rentalContract);
    Task DeleteAsync(int id);
    Task ActivateAsync(int id, int startMileage);
    Task CompleteAsync(int id, DateTime actualReturnDate, int endMileage);
    Task CancelAsync(int id);
}
