using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Services;

namespace WebUI.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;
    private readonly ILogger<LogoutModel> logger;

    public LogoutModel(IGoBikeApiClient apiClient, ILogger<LogoutModel> logger)
    {
        this.apiClient = apiClient;
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        await apiClient.LogoutAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        logger.LogInformation("User {Username} logged out", username);
        return RedirectToPage("/Index");
    }
}
