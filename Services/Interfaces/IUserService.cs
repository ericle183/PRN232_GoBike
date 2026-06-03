using BusinessObjects.DTOs;
using BusinessObjects.Entities;

namespace Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<List<UserDto>> GetAllStaffAsync();
    Task<UserDto?> GetStaffByIdAsync(int id);
    Task<(UserDto? User, string? Error)> CreateStaffAsync(CreateStaffUserRequest request);
    Task<(UserDto? User, string? Error)> UpdateStaffAsync(int id, UpdateStaffUserRequest request);
    Task<(bool Success, string? Error)> DeleteStaffAsync(int id);
}
