using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Services;

namespace WebUI.Helpers;

public static class ApiPageHelper
{
    public static async Task<IActionResult?> HandleApiAuthFailureAsync(
        PageModel page,
        string? error,
        IGoBikeApiClient apiClient)
    {
        if (string.IsNullOrEmpty(error))
            return null;

        if (!error.Contains("đăng nhập lại", StringComparison.OrdinalIgnoreCase))
            return null;

        await apiClient.LogoutAsync();
        await page.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return page.RedirectToPage("/Account/Login");
    }
}
