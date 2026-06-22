using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<CustomerItem> Customers { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public string? SearchQuery { get; set; }

    public static string Mask(string value) =>
        value.Length >= 8 ? value[..4] + "****" + value[^4..] : value;

    public async Task OnGetAsync(string? search, int page = 1)
    {
        SearchQuery = search;
        CurrentPage = page < 1 ? 1 : page;

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var url = $"/api/customer?search={Uri.EscapeDataString(search ?? "")}&page={CurrentPage}&pageSize=10";

        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        Customers = JsonSerializer.Deserialize<List<CustomerItem>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CustomerItem>();
        TotalPages = 1;
    }

    public static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}

public class CustomerItem
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Cccd { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}
