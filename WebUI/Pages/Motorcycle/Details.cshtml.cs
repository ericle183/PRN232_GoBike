using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using System.Text.Json;

namespace WebUI.Pages.Motorcycle;

public class DetailsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public DetailsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public MotorcycleDetailDto Motorcycle { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.GetAsync($"api/motorcycles/{id}");

        if (!response.IsSuccessStatusCode)
            return RedirectToPage("./Index");

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<MotorcycleDetailDto>(json, JsonOptions);

        if (result == null)
            return RedirectToPage("./Index");

        Motorcycle = result;
        return Page();
    }
}
