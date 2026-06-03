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
        var redirect = ApiPageHelper.RedirectIfApiSessionExpired(this, error);
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
        var redirect = ApiPageHelper.RedirectIfApiSessionExpired(this, error);
        if (redirect != null)
            return redirect;

        if (!success)
            return RedirectToPage(new { message = error });

        return RedirectToPage(new { message = "Staff user deleted successfully" });
    }
}
