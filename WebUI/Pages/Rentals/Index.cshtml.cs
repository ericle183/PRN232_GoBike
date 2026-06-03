using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.DTOs;

namespace WebUI.Pages.Rentals;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<RentalListItem> Rentals { get; set; } = new();
    public List<CustomerOption> Customers { get; set; } = new();
    public List<MotorcycleOption> Motorcycles { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? FilterCustomerId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? FilterMotorcycleId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? FilterStatus { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FilterFromDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FilterToDate { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var custRes = await client.GetAsync("/api/customer");
        if (custRes.IsSuccessStatusCode)
        {
            var custJson = await custRes.Content.ReadAsStringAsync();
            Customers = JsonSerializer.Deserialize<List<CustomerOption>>(custJson, jsonOptions) ?? new();
        }

        var motoRes = await client.GetAsync("/api/motorcycles?page=1&pageSize=100");
        if (motoRes.IsSuccessStatusCode)
        {
            var motoJson = await motoRes.Content.ReadAsStringAsync();
            var page = JsonSerializer.Deserialize<PaginatedResult<MotorcycleDto>>(motoJson, jsonOptions);
            Motorcycles = page?.Items.Select(m => new MotorcycleOption
            {
                Id = m.Id,
                LicensePlate = m.LicensePlate,
                Brand = m.Brand,
                Model = m.Model,
                DailyRate = m.DailyRate,
                Mileage = m.Mileage
            }).ToList() ?? new();
        }

        var queryParams = new List<string>();
        if (FilterCustomerId.HasValue) queryParams.Add($"customerId={FilterCustomerId}");
        if (FilterMotorcycleId.HasValue) queryParams.Add($"motorcycleId={FilterMotorcycleId}");
        if (!string.IsNullOrEmpty(FilterStatus)) queryParams.Add($"status={FilterStatus}");
        if (FilterFromDate.HasValue) queryParams.Add($"fromDate={FilterFromDate:yyyy-MM-dd}");
        if (FilterToDate.HasValue) queryParams.Add($"toDate={FilterToDate:yyyy-MM-dd}");

        var url = "/api/rentalcontract" + (queryParams.Any() ? "?" + string.Join("&", queryParams) : "");
        var rentalRes = await client.GetAsync(url);

        if (rentalRes.IsSuccessStatusCode)
        {
            var rentalJson = await rentalRes.Content.ReadAsStringAsync();
            Rentals = JsonSerializer.Deserialize<List<RentalListItem>>(rentalJson, jsonOptions) ?? new();
        }

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
        1 => "Pending",
        2 => "Active",
        3 => "Completed",
        4 => "Cancelled",
        _ => "Unknown"
    };
}
