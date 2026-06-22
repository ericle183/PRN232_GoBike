using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

[Authorize(Roles = "Admin")]
public class ActivateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ActivateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public RentalDetail Rental { get; set; } = new();
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public int StartMileage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/rentalcontract/{id}");

        if (!response.IsSuccessStatusCode) return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        Rental = JsonSerializer.Deserialize<RentalDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RentalDetail();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (StartMileage < 0)
        {
            ModelState.AddModelError(nameof(StartMileage), "Start mileage must be 0 or greater.");
            return await OnGetAsync(id);
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PutAsync($"/api/rentalcontract/{id}/activate?startMileage={StartMileage}", null);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Failed to activate rental contract.";
            return await OnGetAsync(id);
        }

        TempData?["SuccessMessage"] = "Rental contract activated successfully.";
        return RedirectToPage("./Details", new { id });
    }

    public static string GetStatusName(int status) => status switch
    {
        1 => "Pending",
        2 => "Active",
        3 => "Completed",
        4 => "Cancelled",
        _ => "Unknown"
    };
}
