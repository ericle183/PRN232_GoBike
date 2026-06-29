using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.Services.Internal;

namespace WebUI.Pages.Customers;

public class EditModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EditModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public CustomerCreateForm Form { get; set; } = new();

    public int CustomerId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.GetAsync($"/api/customer/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var customer = JsonSerializer.Deserialize<CustomerDetail>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (customer == null) return NotFound();

        CustomerId = customer.Id;
        Form = new CustomerCreateForm
        {
            FullName = customer.FullName,
            Cccd = customer.Cccd,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address,
            DateOfBirth = customer.DateOfBirth,
            DriverLicenseNo = customer.DriverLicenseNo
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            CustomerId = id;
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PutAsJsonAsync($"/api/customer/{id}", Form);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, await ApiResponseReader.ReadErrorMessageAsync(response));
            CustomerId = id;
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
