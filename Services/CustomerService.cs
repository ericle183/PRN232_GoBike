using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    public Task<List<Customer>> GetAllAsync()
        => customerRepository.GetAllAsync();

    public Task<Customer?> GetByIdAsync(int id)
        => customerRepository.GetByIdAsync(id);

    public async Task CreateAsync(Customer customer)
    {
        ValidateCustomer(customer);

        if (await customerRepository.ExistsByCccdAsync(customer.CCCD))
        {
            throw new InvalidOperationException("CCCD already exists in the system");
        }

        if (await customerRepository.ExistsByDriverLicenseNoAsync(customer.DriverLicenseNo))
        {
            throw new InvalidOperationException("Driver license number already exists in the system");
        }

        await customerRepository.AddAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        ValidateCustomer(customer);

        if (await customerRepository.ExistsByCccdAsync(customer.CCCD, customer.Id))
        {
            throw new InvalidOperationException("CCCD already exists in the system");
        }

        if (await customerRepository.ExistsByDriverLicenseNoAsync(customer.DriverLicenseNo, customer.Id))
        {
            throw new InvalidOperationException("Driver license number already exists in the system");
        }

        customer.UpdatedAt = DateTime.UtcNow;
        customerRepository.Update(customer);
    }

    public async Task DeleteAsync(int id)
        => await DeactivateAsync(id);

    public async Task DeactivateAsync(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Customer with ID {id} not found");

        customer.IsActive = false;
        customer.UpdatedAt = DateTime.UtcNow;
        customerRepository.Update(customer);
    }

    public async Task ReactivateAsync(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Customer with ID {id} not found");

        customer.IsActive = true;
        customer.UpdatedAt = DateTime.UtcNow;
        customerRepository.Update(customer);
    }

    private static void ValidateCustomer(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FullName))
        {
            throw new InvalidOperationException("FullName is required");
        }

        if (string.IsNullOrWhiteSpace(customer.CCCD) || customer.CCCD.Length != 12 || !customer.CCCD.All(char.IsDigit))
        {
            throw new InvalidOperationException("CCCD must be exactly 12 digits");
        }

        if (string.IsNullOrWhiteSpace(customer.PhoneNumber) || !System.Text.RegularExpressions.Regex.IsMatch(customer.PhoneNumber, @"^0[0-9]{9,10}$"))
        {
            throw new InvalidOperationException("Invalid Vietnamese phone format");
        }

        if (string.IsNullOrWhiteSpace(customer.DriverLicenseNo))
        {
            throw new InvalidOperationException("DriverLicenseNo is required");
        }

        var age = DateTime.Today.Year - customer.DateOfBirth.Year -
                  (customer.DateOfBirth.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - customer.DateOfBirth.Year)) ? 1 : 0);
        if (age < 18)
        {
            throw new InvalidOperationException("Customer must be at least 18 years old");
        }
    }
}
