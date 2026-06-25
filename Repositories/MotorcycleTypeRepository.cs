using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class MotorcycleTypeRepository : Repository<MotorcycleType>, IMotorcycleTypeRepository
{
    public MotorcycleTypeRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        => dbSet.AnyAsync(x => x.Name == name && (!excludeId.HasValue || x.Id != excludeId.Value));

    public Task<bool> HasActiveMotorcyclesAsync(int id)
        => context.Motorcycles.AnyAsync(x => x.VehicleTypeId == id && x.IsActive);
}
