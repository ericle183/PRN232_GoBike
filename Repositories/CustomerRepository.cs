using BusinessObjects.Entities;
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
}
