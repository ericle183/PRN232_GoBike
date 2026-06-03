using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
