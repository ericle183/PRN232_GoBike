using System.Security.Claims;
using BusinessObjects.DTOs;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Services;

namespace WebUI.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;
    private readonly ILogger<LoginModel> logger;

    public LoginModel(IGoBikeApiClient apiClient, ILogger<LoginModel> logger)
    {
        this.apiClient = apiClient;
        this.logger = logger;
    }

    [BindProperty]
    public LoginRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToRoleHome();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
        {
            ErrorMessage = "Username and password are required";
            return Page();
        }

        var (success, user, error) = await apiClient.LoginAsync(Input);
        if (!success || user == null)
        {
            ErrorMessage = error ?? "Invalid username or password";
            logger.LogWarning("Failed login attempt for username: {Username}", Input.Username);
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.GivenName, user.FullName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

        logger.LogInformation("User {Username} logged in via API", user.Username);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return user.Role == UserRole.Admin
            ? RedirectToPage("/Admin/Staff/Index")
            : RedirectToPage("/Index");
    }

    private IActionResult RedirectToRoleHome()
    {
        return User.IsInRole(UserRole.Admin.ToString())
            ? RedirectToPage("/Admin/Staff/Index")
            : RedirectToPage("/Index");
    }
}
