using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Helpers;
using WebUI.Services;

namespace WebUI.Pages.Admin.Staff;

public class CreateModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;

    public CreateModel(IGoBikeApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [BindProperty]
    public CreateStaffUserRequest Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var (success, _, error) = await apiClient.CreateStaffUserAsync(Input);
        var redirect = ApiPageHelper.RedirectIfApiSessionExpired(this, error);
        if (redirect != null)
            return redirect;

        if (!success)
        {
            ErrorMessage = error ?? "Failed to create staff user";
            return Page();
        }

        return RedirectToPage("Index", new { message = "Staff user created successfully" });
    }
}
