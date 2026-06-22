using BusinessObjects.Entities;

namespace Repositories;

public interface IRentalContractRepository : IRepository<RentalContract>
{
    Task<List<RentalContract>> GetByCustomerIdAsync(int customerId);
    Task<List<RentalContract>> GetByMotorcycleIdAsync(int motorcycleId);
    Task<List<RentalContract>> SearchAsync(int? customerId, int? motorcycleId, BusinessObjects.Enums.RentalStatus? status, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
}
