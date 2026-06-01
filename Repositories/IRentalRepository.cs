using BusinessObjects;

namespace Repositories;

public interface IRentalRepository : IRepository<RentalContract>
{
    Task<IEnumerable<RentalContract>> SearchAsync(
        RentalStatus? status,
        int? customerId,
        int? motorcycleId,
        DateTime? dateFrom,
        DateTime? dateTo,
        int page,
        int pageSize);
    Task<int> GetTotalCountAsync(
        RentalStatus? status,
        int? customerId,
        int? motorcycleId,
        DateTime? dateFrom,
        DateTime? dateTo);
    Task<RentalContract?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<RentalContract>> GetRecentRentalsAsync(int count);
}
