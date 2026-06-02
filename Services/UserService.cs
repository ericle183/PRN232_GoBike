using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task<User?> GetByUsernameAsync(string username)
        => userRepository.GetByUsernameAsync(username);
}
