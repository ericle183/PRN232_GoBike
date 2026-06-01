using BusinessObjects;

namespace Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate);
    Task<IEnumerable<Motorcycle>> GetByStatusAsync(MotorcycleStatus status);
    Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync();
    Task<IEnumerable<Motorcycle>> SearchAsync(string? searchTerm, MotorcycleStatus? status, int page, int pageSize);
    Task<int> GetTotalCountAsync(string? searchTerm, MotorcycleStatus? status);
}
