using BusinessObjects.Entities;

namespace Repositories;

public interface IRentalContractRepository : IRepository<RentalContract>
{
    Task<List<RentalContract>> GetByCustomerIdAsync(int customerId);
    Task<List<RentalContract>> GetByMotorcycleIdAsync(int motorcycleId);
}
