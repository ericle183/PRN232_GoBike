using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace WebUI.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserService userService;
    private readonly ILogger<ProfileModel> logger;

    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    public class UserProfileInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public UserProfileInfo UserProfile { get; set; }
    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await userService.GetByUsernameAsync(username);
            if (user == null)
            {
                ErrorMessage = "User not found";
                return Page();
            }

            UserProfile = new UserProfileInfo
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            logger.LogInformation($"User {username} viewed profile");
            return Page();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading user profile");
            ErrorMessage = "An error occurred while loading profile";
            return Page();
        }
    }
}
