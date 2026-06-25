using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers;

public class DeleteModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeleteModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public CustomerDetail Customer { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/customer/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        Customer = JsonSerializer.Deserialize<CustomerDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new CustomerDetail();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.DeleteAsync($"/api/customer/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            ErrorMessage = "Unable to delete customer. The customer may have rental history associated with them.";
            return await OnGetAsync(id);
        }

        return RedirectToPage("./Index");
    }
}
