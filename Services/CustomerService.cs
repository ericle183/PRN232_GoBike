using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository customerRepository;
    private readonly IRentalContractRepository rentalContractRepository;

    public CustomerService(ICustomerRepository customerRepository, IRentalContractRepository rentalContractRepository)
    {
        this.customerRepository = customerRepository;
        this.rentalContractRepository = rentalContractRepository;
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

        await customerRepository.AddAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        ValidateCustomer(customer);

        if (await customerRepository.ExistsByCccdAsync(customer.CCCD, customer.Id))
        {
            throw new InvalidOperationException("CCCD already exists in the system");
        }

        customer.UpdatedAt = DateTime.UtcNow;
        customerRepository.Update(customer);
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Customer with ID {id} not found");

        var rentals = await rentalContractRepository.GetByCustomerIdAsync(id);
        if (rentals.Count > 0)
        {
            throw new InvalidOperationException("Cannot delete customer who has rental history");
        }

        customer.IsActive = false;
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

        var age = DateTime.Today.Year - customer.DateOfBirth.Year -
                  (customer.DateOfBirth.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - customer.DateOfBirth.Year)) ? 1 : 0);
        if (age < 18)
        {
            throw new InvalidOperationException("Customer must be at least 18 years old");
        }
    }
}
