using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

[Authorize(Roles = "Admin")]
public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public RentalCreateForm Form { get; set; } = new();

    public List<AvailableMotorcycle> AvailableMotorcycles { get; set; } = new();
    public List<CustomerOption> Customers { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var motoRes = await client.GetAsync("/api/motorcycles/available");
        if (motoRes.IsSuccessStatusCode)
        {
            var json = await motoRes.Content.ReadAsStringAsync();
            AvailableMotorcycles = JsonSerializer.Deserialize<List<AvailableMotorcycle>>(json, jsonOptions) ?? new();
        }

        var custRes = await client.GetAsync("/api/customer");
        if (custRes.IsSuccessStatusCode)
        {
            var json = await custRes.Content.ReadAsStringAsync();
            Customers = JsonSerializer.Deserialize<List<CustomerOption>>(json, jsonOptions) ?? new();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return await OnGetAsync();
        }

        if (Form.ExpectedReturnDate <= Form.RentalDate)
        {
            ModelState.AddModelError("Form.ExpectedReturnDate", "Expected return date must be after rental date.");
            return await OnGetAsync();
        }

        var payload = new
        {
            customerId = Form.CustomerId,
            motorcycleId = Form.MotorcycleId,
            rentalDate = Form.RentalDate.ToString("yyyy-MM-dd"),
            expectedReturnDate = Form.ExpectedReturnDate.ToString("yyyy-MM-dd"),
            depositAmount = Form.DepositAmount,
            notes = Form.Notes,
            createdBy = "Admin"
        };

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PostAsJsonAsync("/api/rentalcontract", payload);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to create rental contract. Please try again.");
            return await OnGetAsync();
        }

        var resultJson = await response.Content.ReadAsStringAsync();
        var created = JsonSerializer.Deserialize<RentalCreatedResult>(resultJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        TempData?["SuccessMessage"] = "Rental contract created successfully.";
        return RedirectToPage("./Details", new { id = created?.Id ?? 0 });
    }
}
