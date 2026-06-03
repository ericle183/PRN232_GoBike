using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using WebUI.Services;

namespace WebUI.Pages.Motorcycle;

public class DetailsModel : PageModel
{
    private readonly IGoBikeApiClient _apiClient;

    public DetailsModel(IGoBikeApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public MotorcycleDetailDto Motorcycle { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var (success, motorcycle, error) = await _apiClient.GetMotorcycleAsync(id);

        if (!success || motorcycle == null)
        {
            if (!string.IsNullOrEmpty(error))
                TempData["Error"] = error;
            return RedirectToPage("./Index");
        }

        Motorcycle = motorcycle;
        return Page();
    }
}
