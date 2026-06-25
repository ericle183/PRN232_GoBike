using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DetailsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public RentalDetail Rental { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/rentalcontract/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        Rental = JsonSerializer.Deserialize<RentalDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RentalDetail();

        return Page();
    }

    public static string GetStatusBadgeClass(int status) => status switch
    {
        2 => "badge-success",
        1 => "badge-warning",
        3 => "badge-neutral",
        4 => "badge-error",
        _ => "badge-info"
    };

    public static string GetStatusName(int status) => status switch
    {
        1 => "Reserved",
        2 => "Active",
        3 => "Completed",
        4 => "Cancelled",
        _ => "Unknown"
    };
}
