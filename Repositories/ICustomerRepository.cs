using BusinessObjects;

namespace Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByCCCDAsync(string cccd);
    Task<IEnumerable<Customer>> SearchAsync(string? searchTerm, int page, int pageSize);
    Task<int> GetTotalCountAsync(string? searchTerm);
    Task<bool> HasRentalHistoryAsync(int customerId);
}
