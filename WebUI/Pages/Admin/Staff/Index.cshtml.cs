using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Helpers;
using WebUI.Services;

namespace WebUI.Pages.Admin.Staff;

public class IndexModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;

    public IndexModel(IGoBikeApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public List<UserDto> StaffUsers { get; set; } = [];
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? message = null)
    {
        SuccessMessage = message;
        var (success, users, error) = await apiClient.GetStaffUsersAsync();
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success)
        {
            ErrorMessage = error ?? "Failed to load staff users";
            return Page();
        }

        StaffUsers = users ?? [];
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var (success, error) = await apiClient.DeleteStaffUserAsync(id);
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success)
            return RedirectToPage(new { message = error });

        return RedirectToPage(new { message = "Staff user deleted successfully" });
    }

    public async Task<IActionResult> OnPostSaveAsync([FromForm] string username, [FromForm] string password, 
        [FromForm] string fullName, [FromForm] string? email, [FromForm] bool isActive)
    {
        var request = new CreateStaffUserRequest
        {
            Username = username.Trim(),
            Password = password,
            FullName = fullName.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            IsActive = isActive
        };

        var (success, user, error) = await apiClient.CreateStaffUserAsync(request);
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success)
        {
            // Return JSON for AJAX
            return new JsonResult(new { success = false, message = error ?? "Failed to create staff user" });
        }

        return new JsonResult(new { success = true, message = "Staff user created successfully" });
    }

    public async Task<IActionResult> OnPutUpdateAsync(int id, [FromForm] string fullName, 
        [FromForm] string? email, [FromForm] string? password, [FromForm] bool isActive)
    {
        var request = new UpdateStaffUserRequest
        {
            FullName = fullName.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            Password = string.IsNullOrWhiteSpace(password) ? null : password,
            IsActive = isActive
        };

        var (success, user, error) = await apiClient.UpdateStaffUserAsync(id, request);
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success)
        {
            // Return JSON for AJAX
            return new JsonResult(new { success = false, message = error ?? "Failed to update staff user" });
        }

        return new JsonResult(new { success = true, message = "Staff user updated successfully" });
    }
}