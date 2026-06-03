using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        var custRes = await client.GetAsync("/api/customer");
        if (custRes.IsSuccessStatusCode)
        {
            var custJson = await custRes.Content.ReadAsStringAsync();
            Customers = JsonSerializer.Deserialize<List<CustomerOption>>(custJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }

        var motoRes = await client.GetAsync("/api/motorcycle");
        if (motoRes.IsSuccessStatusCode)
        {
            var motoJson = await motoRes.Content.ReadAsStringAsync();
            Motorcycles = JsonSerializer.Deserialize<List<MotorcycleOption>>(motoJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
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
            Rentals = JsonSerializer.Deserialize<List<RentalListItem>>(rentalJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
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
