using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;

namespace Repositories;

public class RentalRepository : Repository<RentalContract>, IRentalRepository
{
    public RentalRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RentalContract>> SearchAsync(
        RentalStatus? status,
        int? customerId,
        int? motorcycleId,
        DateTime? dateFrom,
        DateTime? dateTo,
        int page,
        int pageSize)
    {
        var query = _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Motorcycle)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        if (customerId.HasValue)
            query = query.Where(r => r.CustomerId == customerId.Value);

        if (motorcycleId.HasValue)
            query = query.Where(r => r.MotorcycleId == motorcycleId.Value);

        if (dateFrom.HasValue)
            query = query.Where(r => r.RentalDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(r => r.RentalDate <= dateTo.Value);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(
        RentalStatus? status,
        int? customerId,
        int? motorcycleId,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        var query = _dbSet.AsQueryable();

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        if (customerId.HasValue)
            query = query.Where(r => r.CustomerId == customerId.Value);

        if (motorcycleId.HasValue)
            query = query.Where(r => r.MotorcycleId == motorcycleId.Value);

        if (dateFrom.HasValue)
            query = query.Where(r => r.RentalDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(r => r.RentalDate <= dateTo.Value);

        return await query.CountAsync();
    }

    public async Task<RentalContract?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Motorcycle)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<RentalContract>> GetRecentRentalsAsync(int count)
    {
        return await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Motorcycle)
            .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public override async Task<IEnumerable<RentalContract>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Motorcycle)
            .ToListAsync();
    }
}
