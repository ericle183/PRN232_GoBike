using System.Net.Http.Json;
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

    [BindProperty]
    public string Reason { get; set; } = "";

    [BindProperty]
    public int RefundPaymentMethod { get; set; } = 1;

    [BindProperty]
    public string? RefundPaymentNote { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return await LoadRentalAsync(id) ? Page() : NotFound();
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (string.IsNullOrWhiteSpace(Reason))
        {
            ModelState.AddModelError(nameof(Reason), "Cancellation reason is required.");
            await LoadRentalAsync(id);
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var payload = new
        {
            reason = Reason,
            refundPaymentMethod = RefundPaymentMethod,
            refundPaymentNote = RefundPaymentNote
        };

        var response = await client.PostAsJsonAsync($"/api/rental-contracts/{id}/cancel", payload);

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = await ReadErrorMessageAsync(response, "Failed to cancel rental contract.");
            await LoadRentalAsync(id);
            return Page();
        }

        TempData?["SuccessMessage"] = "Rental contract cancelled successfully.";
        return RedirectToPage("./Index");
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
}
