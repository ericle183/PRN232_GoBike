using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public CustomerCreateForm Form { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _httpClientFactory.CreateClient("GobikeApi");
        var response = await client.PostAsJsonAsync("/api/customer", Form);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Failed to create customer. Please check your input.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}

public class CustomerCreateForm
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "CCCD is required.")]
    [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "CCCD must be exactly 12 digits.")]
    public string Cccd { get; set; } = "";

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^0[0-9]{9,10}$", ErrorMessage = "Phone must be 10-11 digits starting with 0.")]
    public string PhoneNumber { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = "";

    public string? Address { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    public DateTime DateOfBirth { get; set; }

    public string? DriverLicenseNo { get; set; }
}
