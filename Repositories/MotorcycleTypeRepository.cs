using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;

namespace Repositories;

public class MotorcycleTypeRepository : Repository<MotorcycleType>, IMotorcycleTypeRepository
{
    public MotorcycleTypeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MotorcycleType>> GetAllActiveAsync()
    {
        return await _dbSet.Where(m => m.IsActive).ToListAsync();
    }

    public override async Task<IEnumerable<MotorcycleType>> GetAllAsync()
    {
        return await _dbSet.IgnoreQueryFilters().ToListAsync();
    }
}
