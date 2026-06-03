using BusinessObjects.Entities;
using BusinessObjects.Enums;

namespace Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<List<Motorcycle>> GetAvailableAsync();
    Task<bool> ExistsByLicensePlateAsync(string licensePlate, int? excludeId = null);
    Task<(List<Motorcycle> Items, int TotalCount)> SearchAsync(
        string? searchTerm,
        MotorcycleStatus? status,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize);
    Task<Motorcycle?> GetByIdWithDetailsAsync(int id);
    Task<bool> HasActiveRentalsAsync(int motorcycleId);
}
