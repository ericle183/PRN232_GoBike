using System.Security.Claims;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace WebUI.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService userService;
    private readonly ILogger<LoginModel> logger;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    [BindProperty]
    public LoginRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
        {
            ErrorMessage = "Username and password are required";
            return Page();
        }

        var user = await userService.GetByUsernameAsync(Input.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(Input.Password, user.PasswordHash))
        {
            ErrorMessage = "Invalid username or password";
            logger.LogWarning($"Failed login attempt for username: {Input.Username}");
            return Page();
        }

        if (!user.IsActive)
        {
            ErrorMessage = "User account is inactive";
            return Page();
        }

        // Create claims for the user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        logger.LogInformation($"User {user.Username} logged in successfully");

        return LocalRedirect(returnUrl ?? "/");
    }
}
