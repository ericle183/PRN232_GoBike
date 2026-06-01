using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;

namespace Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _dbSet.FirstOrDefaultAsync(m => m.LicensePlate == licensePlate);
    }

    public async Task<IEnumerable<Motorcycle>> GetByStatusAsync(MotorcycleStatus status)
    {
        return await _dbSet.Where(m => m.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<Motorcycle>> GetAvailableMotorcyclesAsync()
    {
        return await _dbSet.Where(m => m.Status == MotorcycleStatus.Available).ToListAsync();
    }

    public async Task<IEnumerable<Motorcycle>> SearchAsync(string? searchTerm, MotorcycleStatus? status, int page, int pageSize)
    {
        var query = _dbSet.Include(m => m.VehicleType).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(m =>
                m.LicensePlate.ToLower().Contains(searchTerm) ||
                m.Brand.ToLower().Contains(searchTerm) ||
                m.Model.ToLower().Contains(searchTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(m => m.Status == status.Value);
        }

        return await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm, MotorcycleStatus? status)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(m =>
                m.LicensePlate.ToLower().Contains(searchTerm) ||
                m.Brand.ToLower().Contains(searchTerm) ||
                m.Model.ToLower().Contains(searchTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(m => m.Status == status.Value);
        }

        return await query.CountAsync();
    }
}
