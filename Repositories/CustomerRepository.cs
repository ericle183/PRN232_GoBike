using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByCccdAsync(string cccd, int? excludeId = null)
        => dbSet.AnyAsync(x => x.CCCD == cccd && (!excludeId.HasValue || x.Id != excludeId.Value));

    public Task<bool> ExistsByDriverLicenseNoAsync(string driverLicenseNo, int? excludeId = null)
        => dbSet.AnyAsync(x => x.DriverLicenseNo == driverLicenseNo && (!excludeId.HasValue || x.Id != excludeId.Value));

    public Task<List<Customer>> SearchAsync(string? keyword, int page, int pageSize)
    {
        var query = dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim().ToLower();
            query = query.Where(x =>
                x.FullName.ToLower().Contains(keyword) ||
                x.CCCD.ToLower().Contains(keyword) ||
                x.PhoneNumber.ToLower().Contains(keyword) ||
                x.DriverLicenseNo.ToLower().Contains(keyword));
        }

        return query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
