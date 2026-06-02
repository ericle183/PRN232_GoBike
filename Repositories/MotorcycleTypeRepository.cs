using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class MotorcycleTypeRepository : Repository<MotorcycleType>, IMotorcycleTypeRepository
{
    public MotorcycleTypeRepository(AppDbContext context) : base(context)
    {
    }
}
