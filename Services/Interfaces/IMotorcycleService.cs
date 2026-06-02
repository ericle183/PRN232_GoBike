using BusinessObjects.Entities;

namespace Services.Interfaces;

public interface IMotorcycleService
{
    Task<List<Motorcycle>> GetAllAsync();
    Task<Motorcycle?> GetByIdAsync(int id);
    Task<List<Motorcycle>> GetAvailableAsync();
    Task CreateAsync(Motorcycle motorcycle);
    Task UpdateAsync(Motorcycle motorcycle);
    Task DeleteAsync(int id);
}
