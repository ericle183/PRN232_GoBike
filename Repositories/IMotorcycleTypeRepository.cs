using BusinessObjects;

namespace Repositories;

public interface IMotorcycleTypeRepository : IRepository<MotorcycleType>
{
    Task<IEnumerable<MotorcycleType>> GetAllActiveAsync();
}
