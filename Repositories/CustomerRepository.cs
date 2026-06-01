using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;

namespace Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByCCCDAsync(string cccd)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.CCCD == cccd);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string? searchTerm, int page, int pageSize)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(c =>
                c.FullName.ToLower().Contains(searchTerm) ||
                c.CCCD.Contains(searchTerm) ||
                c.PhoneNumber.Contains(searchTerm));
        }

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(c =>
                c.FullName.ToLower().Contains(searchTerm) ||
                c.CCCD.Contains(searchTerm) ||
                c.PhoneNumber.Contains(searchTerm));
        }

        return await query.CountAsync();
    }

    public async Task<bool> HasRentalHistoryAsync(int customerId)
    {
        return await _context.RentalContracts
            .AnyAsync(r => r.CustomerId == customerId);
    }
}
