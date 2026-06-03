using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;
using System.Text.Json;

namespace WebUI.Pages.Motorcycle;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public PaginatedResult<MotorcycleDto> Result { get; set; } = new();
    public List<MotorcycleTypeItem> VehicleTypes { get; set; } = [];

    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public MotorcycleStatus? Status { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MaxPrice { get; set; }
    [BindProperty(SupportsGet = true)] public int Page { get; set; } = 1;

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("API");

        var query = $"api/motorcycles?page={Page}&pageSize=10";
        if (!string.IsNullOrWhiteSpace(Search)) query += $"&search={Uri.EscapeDataString(Search)}";
        if (Status.HasValue) query += $"&status={Status}";
        if (MinPrice.HasValue) query += $"&minPrice={MinPrice}";
        if (MaxPrice.HasValue) query += $"&maxPrice={MaxPrice}";

        var response = await client.GetAsync(query);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Result = JsonSerializer.Deserialize<PaginatedResult<MotorcycleDto>>(json, JsonOptions)
                     ?? new PaginatedResult<MotorcycleDto>();
        }

        await LoadVehicleTypes(client);
    }

    public async Task<IActionResult> OnPostCreateAsync([FromForm] CreateMotorcycleRequest request)
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.PostAsJsonAsync("api/motorcycles", request);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Motorcycle created successfully!";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = TryExtractMessage(error);
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync(int id, [FromForm] UpdateMotorcycleRequest request)
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.PutAsJsonAsync($"api/motorcycles/{id}", request);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Motorcycle updated successfully!";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = TryExtractMessage(error);
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.DeleteAsync($"api/motorcycles/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Motorcycle deleted successfully!";
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = TryExtractMessage(error);
        }

        return RedirectToPage();
    }

    private async Task LoadVehicleTypes(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("api/motorcycletypes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                VehicleTypes = JsonSerializer.Deserialize<List<MotorcycleTypeItem>>(json, JsonOptions) ?? [];
            }
        }
        catch { }
    }

    private static string TryExtractMessage(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("message").GetString() ?? "An error occurred.";
        }
        catch
        {
            return "An error occurred.";
        }
    }
}

public class MotorcycleTypeItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DefaultDailyRate { get; set; }
}
