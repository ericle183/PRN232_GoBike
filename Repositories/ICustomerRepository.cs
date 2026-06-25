using BusinessObjects.Entities;

namespace Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<bool> ExistsByCccdAsync(string cccd, int? excludeId = null);
    Task<bool> ExistsByDriverLicenseNoAsync(string driverLicenseNo, int? excludeId = null);
    Task<List<Customer>> SearchAsync(string? keyword, int page, int pageSize);
}
