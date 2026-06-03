using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : BaseApiController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("staff")]
    public async Task<IActionResult> GetStaffUsers()
    {
        var users = await userService.GetAllStaffAsync();
        return Ok(users);
    }

    [HttpGet("staff/{id:int}")]
    public async Task<IActionResult> GetStaffUser(int id)
    {
        var user = await userService.GetStaffByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "Staff user not found" });
        return Ok(user);
    }

    [HttpPost("staff")]
    public async Task<IActionResult> CreateStaffUser([FromBody] CreateStaffUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid request data" });

        var (user, error) = await userService.CreateStaffAsync(request);
        if (error != null)
            return BadRequest(new { message = error });
        return CreatedAtAction(nameof(GetStaffUser), new { id = user!.Id }, user);
    }

    [HttpPut("staff/{id:int}")]
    public async Task<IActionResult> UpdateStaffUser(int id, [FromBody] UpdateStaffUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "Invalid request data" });

        var (user, error) = await userService.UpdateStaffAsync(id, request);
        if (error == "Staff user not found")
            return NotFound(new { message = error });
        if (error != null)
            return BadRequest(new { message = error });
        return Ok(user);
    }

    [HttpDelete("staff/{id:int}")]
    public async Task<IActionResult> DeleteStaffUser(int id)
    {
        var (success, error) = await userService.DeleteStaffAsync(id);
        if (!success)
            return NotFound(new { message = error });
        return Ok(new { message = "Staff user deleted successfully" });
    }
}
