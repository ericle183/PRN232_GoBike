using System.Security.Claims;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;

    public AuthController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Username and password are required" });

        var user = await userService.GetByUsernameAsync(request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid username or password" });

        if (!user.IsActive)
            return Unauthorized(new { message = "User account is inactive" });

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

        var response = new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Role = user.Role
        };

        return Ok(new { message = "Login successful", user = response });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logout successful" });
    }

    [AllowAnonymous]
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return Forbid("Access denied");
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await userService.GetByUsernameAsync(User.FindFirst(ClaimTypes.Name)?.Value ?? "");
        if (user == null)
            return NotFound();

        var response = new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email ?? "",
            Role = user.Role
        };

        return Ok(response);
    }
}
