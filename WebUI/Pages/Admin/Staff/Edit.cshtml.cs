using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Helpers;
using WebUI.Services;

namespace WebUI.Pages.Admin.Staff;

public class EditModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;

    public EditModel(IGoBikeApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public int StaffId { get; set; }
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public UpdateStaffUserRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var (success, user, error) = await apiClient.GetStaffUserAsync(id);
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success || user == null)
        {
            ErrorMessage = error ?? "Staff user not found";
            return Page();
        }

        BindUser(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        StaffId = id;

        if (!ModelState.IsValid)
        {
            await LoadUsernameAsync(id);
            return Page();
        }

        var (success, _, error) = await apiClient.UpdateStaffUserAsync(id, Input);
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success)
        {
            ErrorMessage = error ?? "Failed to update staff user";
            await LoadUsernameAsync(id);
            return Page();
        }

        return RedirectToPage("Index", new { message = "Staff user updated successfully" });
    }

    private void BindUser(UserDto user)
    {
        StaffId = user.Id;
        Username = user.Username;
        Input = new UpdateStaffUserRequest
        {
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }

    private async Task LoadUsernameAsync(int id)
    {
        var (success, user, _) = await apiClient.GetStaffUserAsync(id);
        if (success && user != null)
            Username = user.Username;
    }
}
