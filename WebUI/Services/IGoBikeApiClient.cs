using BusinessObjects.DTOs;

namespace WebUI.Services;

public interface IGoBikeApiClient
{
    Task<(bool Success, LoginResponse? User, string? Error)> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<(bool Success, UserProfileDto? Profile, string? Error)> GetProfileAsync();
    Task<(bool Success, List<UserDto>? Users, string? Error)> GetStaffUsersAsync();
    Task<(bool Success, UserDto? User, string? Error)> GetStaffUserAsync(int id);
    Task<(bool Success, UserDto? User, string? Error)> CreateStaffUserAsync(CreateStaffUserRequest request);
    Task<(bool Success, UserDto? User, string? Error)> UpdateStaffUserAsync(int id, UpdateStaffUserRequest request);
    Task<(bool Success, string? Error)> DeleteStaffUserAsync(int id);
}
