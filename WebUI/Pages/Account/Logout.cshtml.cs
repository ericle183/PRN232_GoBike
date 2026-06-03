using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> logger;

    public LogoutModel(ILogger<LogoutModel> logger)
    {
        this.logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        logger.LogInformation($"User {username} logged out successfully");

        return RedirectToPage("/Index");
    }
}
