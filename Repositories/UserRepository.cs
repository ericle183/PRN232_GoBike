using Microsoft.EntityFrameworkCore;
using DataAccessObjects;
using BusinessObjects;

namespace Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet.IgnoreQueryFilters().ToListAsync();
    }
}
