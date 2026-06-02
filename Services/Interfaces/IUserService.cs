using BusinessObjects.Entities;

namespace Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByUsernameAsync(string username);
}
