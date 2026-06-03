using BusinessObjects.Entities;

namespace Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>> GetStaffUsersAsync();
    Task<bool> UsernameExistsAsync(string username, int? excludeUserId = null);
}
