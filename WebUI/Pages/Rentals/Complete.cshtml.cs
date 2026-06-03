using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

public class CompleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CompleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public RentalDetail Rental { get; set; } = new();
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public DateTime ActualReturnDate { get; set; }

    [BindProperty]
    public int EndMileage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/rentalcontract/{id}");

        if (!response.IsSuccessStatusCode) return NotFound();

        var json = await response.Content.ReadAsStringAsync();
        Rental = JsonSerializer.Deserialize<RentalDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RentalDetail();

        ActualReturnDate = DateTime.Today;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (EndMileage < 0)
        {
            ModelState.AddModelError(nameof(EndMileage), "End mileage must be 0 or greater.");
            return await OnGetAsync(id);
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var returnDateStr = ActualReturnDate.ToString("yyyy-MM-dd");
        var response = await client.PutAsync(
            $"/api/rentalcontract/{id}/complete?actualReturnDate={returnDateStr}&endMileage={EndMileage}",
            null);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = "Failed to complete rental contract.";
            return await OnGetAsync(id);
        }

        TempData?["SuccessMessage"] = "Rental contract completed successfully.";
        return RedirectToPage("./Details", new { id });
    }
}
