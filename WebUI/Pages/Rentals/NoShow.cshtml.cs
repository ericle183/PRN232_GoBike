using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Rentals;

[Authorize(Roles = "Admin")]
public class NoShowModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NoShowModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public RentalDetail Rental { get; set; } = new();
    public string? ErrorMessage { get; set; }

    [BindProperty]
    public string Reason { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return await LoadRentalAsync(id) ? Page() : NotFound();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (string.IsNullOrWhiteSpace(Reason))
        {
            ModelState.AddModelError(nameof(Reason), "No-show reason is required.");
            await LoadRentalAsync(id);
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PostAsJsonAsync($"/api/rental-contracts/{id}/no-show", new { reason = Reason });

        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = await ReadErrorMessageAsync(response, "Failed to mark rental as no-show.");
            await LoadRentalAsync(id);
            return Page();
        }

        TempData?["SuccessMessage"] = "Rental contract marked as no-show.";
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
}
