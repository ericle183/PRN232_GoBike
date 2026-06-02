using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class RentalContractRepository : Repository<RentalContract>, IRentalContractRepository
{
    public RentalContractRepository(AppDbContext context) : base(context)
    {
    }

    public Task<List<RentalContract>> GetByCustomerIdAsync(int customerId)
        => dbSet.Where(x => x.CustomerId == customerId).ToListAsync();

    public Task<List<RentalContract>> GetByMotorcycleIdAsync(int motorcycleId)
        => dbSet.Where(x => x.MotorcycleId == motorcycleId).ToListAsync();
}
