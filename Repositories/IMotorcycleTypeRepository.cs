using BusinessObjects.Entities;

namespace Repositories;

public interface IMotorcycleTypeRepository : IRepository<MotorcycleType>
{
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<bool> HasActiveMotorcyclesAsync(int id);
}
