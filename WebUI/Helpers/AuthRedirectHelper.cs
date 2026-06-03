using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Helpers;

public static class AuthRedirectHelper
{
    public static IActionResult RedirectToHome(PageModel page)
        => page.RedirectToPage("/Index");
}
