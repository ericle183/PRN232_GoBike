using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Helpers;

public static class ApiPageHelper
{
    public static IActionResult? RedirectIfApiSessionExpired(PageModel page, string? error)
    {
        if (string.IsNullOrEmpty(error))
            return null;

        if (!error.Contains("đăng nhập lại", StringComparison.OrdinalIgnoreCase))
            return null;

        return page.RedirectToPage("/Account/Login", new
        {
            returnUrl = page.HttpContext.Request.Path + page.HttpContext.Request.QueryString
        });
    }
}
