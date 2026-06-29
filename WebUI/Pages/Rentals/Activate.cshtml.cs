using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

[Authorize(Roles = "Admin,Staff")]
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

    [BindProperty]
    public string FuelLevel { get; set; } = "Full";

    [BindProperty]
    public string VehicleCondition { get; set; } = "Good";

    [BindProperty]
    public bool HasDamage { get; set; }

    [BindProperty]
    public string? DamageDescription { get; set; }

    [BindProperty]
    public string? AccessoriesNote { get; set; }

    [BindProperty]
    public string? InspectionNote { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (!await LoadRentalAsync(id)) return NotFound();

        StartMileage = Rental.StartMileage ?? Rental.MotorcycleMileage ?? 0;
        FuelLevel = "Full";
        VehicleCondition = "Good";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (StartMileage < 0)
        {
            ModelState.AddModelError(nameof(StartMileage), "Start mileage must be 0 or greater.");
            await LoadRentalAsync(id);
            return Page();
        }

        if (string.IsNullOrWhiteSpace(FuelLevel))
        {
            ModelState.AddModelError(nameof(FuelLevel), "Fuel level is required.");
            await LoadRentalAsync(id);
            return Page();
        }

        if (string.IsNullOrWhiteSpace(VehicleCondition))
        {
            ModelState.AddModelError(nameof(VehicleCondition), "Vehicle condition is required.");
            await LoadRentalAsync(id);
            return Page();
        }

        if (HasDamage && string.IsNullOrWhiteSpace(DamageDescription))
        {
            ModelState.AddModelError(nameof(DamageDescription), "Damage description is required when damage is reported.");
            await LoadRentalAsync(id);
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var payload = new
        {
            beforeInspection = new
            {
                mileage = StartMileage,
                fuelLevel = FuelLevel,
                vehicleCondition = VehicleCondition,
                hasDamage = HasDamage,
                damageDescription = DamageDescription,
                accessoriesNote = AccessoriesNote,
                note = InspectionNote
            }
        };

        var response = await client.PostAsJsonAsync($"/api/rental-contracts/{id}/handover", payload);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = await ReadErrorMessageAsync(response, "Failed to activate rental contract.");
            await LoadRentalAsync(id);
            return Page();
        }

        TempData?["SuccessMessage"] = "Rental contract activated successfully.";
        return RedirectToPage("./Details", new { id });
    }

    private async Task<bool> LoadRentalAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/rental-contracts/{id}");

        if (!response.IsSuccessStatusCode) return false;

        var json = await response.Content.ReadAsStringAsync();
        Rental = JsonSerializer.Deserialize<RentalDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new RentalDetail();

        return true;
    }

    private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response, string fallback)
    {
        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(json)) return fallback;

        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? fallback;

            if (document.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? fallback;

            if (document.RootElement.TryGetProperty("errors", out var errors))
                return errors.ToString();
        }
        catch (JsonException)
        {
            return json;
        }

        return fallback;
    }

    public static string GetStatusName(int status) => status switch
    {
        1 => "Reserved",
        2 => "Active",
        3 => "Completed",
        4 => "Cancelled",
        5 => "NoShow",
        _ => "Unknown"
    };
}
