using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public Task<User?> GetByUsernameAsync(string username)
        => dbSet.FirstOrDefaultAsync(x => x.Username == username);

    public Task<List<User>> GetStaffUsersAsync()
        => dbSet.Where(x => x.Role == UserRole.Staff).OrderBy(x => x.Username).ToListAsync();

    public Task<bool> UsernameExistsAsync(string username, int? excludeUserId = null)
    {
        var query = dbSet.Where(x => x.Username == username);
        if (excludeUserId.HasValue)
            query = query.Where(x => x.Id != excludeUserId.Value);
        return query.AnyAsync();
    }
}
