using BusinessObjects.Entities;

namespace Repositories;

public interface IMotorcycleRepository : IRepository<Motorcycle>
{
    Task<List<Motorcycle>> GetAvailableAsync();
    Task<bool> ExistsByLicensePlateAsync(string licensePlate, int? excludeId = null);
}
