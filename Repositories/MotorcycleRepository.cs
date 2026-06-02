using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class MotorcycleRepository : Repository<Motorcycle>, IMotorcycleRepository
{
    public MotorcycleRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<Motorcycle>> GetAvailableAsync()
        => dbSet.Where(x => x.Status == MotorcycleStatus.Available).ToListAsync();

    public Task<bool> ExistsByLicensePlateAsync(string licensePlate, int? excludeId = null)
        => dbSet.AnyAsync(x => x.LicensePlate == licensePlate && (!excludeId.HasValue || x.Id != excludeId.Value));
}
