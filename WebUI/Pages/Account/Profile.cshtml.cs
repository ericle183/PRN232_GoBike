using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Helpers;
using WebUI.Services;

namespace WebUI.Pages.Account;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IGoBikeApiClient apiClient;
    private readonly ILogger<ProfileModel> logger;

    public ProfileModel(IGoBikeApiClient apiClient, ILogger<ProfileModel> logger)
    {
        this.apiClient = apiClient;
        this.logger = logger;
    }

    public UserProfileDto? UserProfile { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var (success, profile, error) = await apiClient.GetProfileAsync();
        var redirect = await ApiPageHelper.HandleApiAuthFailureAsync(this, error, apiClient);
        if (redirect != null)
            return redirect;

        if (!success || profile == null)
        {
            ErrorMessage = error ?? "Unable to load profile from API";
            return Page();
        }

        UserProfile = profile;
        logger.LogInformation("User {Username} viewed profile via API", profile.Username);
        return Page();
    }
}
