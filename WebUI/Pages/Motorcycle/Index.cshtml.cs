using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using WebUI.Services;

namespace WebUI.Pages.Motorcycle;

public class IndexModel : PageModel
{
    private readonly IGoBikeApiClient _apiClient;

    public IndexModel(IGoBikeApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public PaginatedResult<MotorcycleDto> Result { get; set; } = new();
    public List<MotorcycleTypeDto> VehicleTypes { get; set; } = [];

    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public MotorcycleStatus? Status { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MaxPrice { get; set; }
    [BindProperty(SupportsGet = true, Name = "page")] public int PageNumber { get; set; } = 1;

    public async Task OnGetAsync()
    {
        var (success, result, error) = await _apiClient.GetMotorcyclesAsync(
            Search, Status, MinPrice, MaxPrice, PageNumber);

        if (success && result != null)
            Result = result;
        else if (!string.IsNullOrEmpty(error))
            TempData["Error"] = error;

        var (typesSuccess, types, typesError) = await _apiClient.GetMotorcycleTypesAsync();
        if (typesSuccess && types != null)
            VehicleTypes = types;
        else if (!string.IsNullOrEmpty(typesError) && TempData["Error"] == null)
            TempData["Error"] = typesError;
    }

    public async Task<IActionResult> OnPostCreateAsync([FromForm] CreateMotorcycleRequest request)
    {
        if (!User.IsInRole("Admin"))
            return Forbid();

        var (success, _, error) = await _apiClient.CreateMotorcycleAsync(request);
        TempData[success ? "Success" : "Error"] = success
            ? "Motorcycle created successfully!"
            : error ?? "Failed to create motorcycle.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync(int id, [FromForm] UpdateMotorcycleRequest request)
    {
        var (success, _, error) = await _apiClient.UpdateMotorcycleAsync(id, request);
        TempData[success ? "Success" : "Error"] = success
            ? "Motorcycle updated successfully!"
            : error ?? "Failed to update motorcycle.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (!User.IsInRole("Admin"))
            return Forbid();

        var (success, error) = await _apiClient.DeleteMotorcycleAsync(id);
        TempData[success ? "Success" : "Error"] = success
            ? "Motorcycle deleted successfully!"
            : error ?? "Failed to delete motorcycle.";
        return RedirectToPage();
    }
}
