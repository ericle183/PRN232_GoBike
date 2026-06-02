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
        await customerRepository.AddAsync(customer);
    }

    public Task UpdateAsync(Customer customer)
    {
        customerRepository.Update(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        return Task.CompletedTask;
    }
}
