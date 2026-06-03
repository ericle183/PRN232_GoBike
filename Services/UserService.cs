using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
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

    public Task<User?> GetByIdAsync(int id)
        => userRepository.GetByIdAsync(id);

    public async Task<List<UserDto>> GetAllStaffAsync()
    {
        var users = await userRepository.GetStaffUsersAsync();
        return users.Select(MapToDto).ToList();
    }

    public async Task<UserDto?> GetStaffByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null || user.Role != UserRole.Staff)
            return null;
        return MapToDto(user);
    }

    public async Task<(UserDto? User, string? Error)> CreateStaffAsync(CreateStaffUserRequest request)
    {
        if (await userRepository.UsernameExistsAsync(request.Username))
            return (null, "Username already exists");

        var user = new User
        {
            Username = request.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 11),
            FullName = request.FullName.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Role = UserRole.Staff,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user);
        return (MapToDto(user), null);
    }

    public async Task<(UserDto? User, string? Error)> UpdateStaffAsync(int id, UpdateStaffUserRequest request)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null || user.Role != UserRole.Staff)
            return (null, "Staff user not found");

        user.FullName = request.FullName.Trim();
        user.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 11);

        userRepository.Update(user);
        return (MapToDto(user), null);
    }

    public async Task<(bool Success, string? Error)> DeleteStaffAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null || user.Role != UserRole.Staff)
            return (false, "Staff user not found");

        userRepository.Delete(user);
        return (true, null);
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}
