using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

[Authorize(Roles = "Admin")]
public class CancelModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CancelModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public RentalDetail Rental { get; set; } = new();
    public string? ErrorMessage { get; set; }

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

    public static string GetStatusName(int status) => status switch
    {
        1 => "Reserved",
        2 => "Active",
        3 => "Completed",
        4 => "Cancelled",
        _ => "Unknown"
    };

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PutAsync($"/api/rentalcontract/{id}/cancel", null);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Failed to cancel rental contract.";
            return await OnGetAsync(id);
        }

        TempData?["SuccessMessage"] = "Rental contract cancelled successfully.";
        return RedirectToPage("./Index");
    }
}
