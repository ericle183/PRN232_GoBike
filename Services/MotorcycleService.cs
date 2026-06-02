using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository motorcycleRepository;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository)
    {
        this.motorcycleRepository = motorcycleRepository;
    }

    public Task<List<Motorcycle>> GetAllAsync()
        => motorcycleRepository.GetAllAsync();

    public Task<Motorcycle?> GetByIdAsync(int id)
        => motorcycleRepository.GetByIdAsync(id);

    public Task<List<Motorcycle>> GetAvailableAsync()
        => motorcycleRepository.GetAvailableAsync();

    public async Task CreateAsync(Motorcycle motorcycle)
    {
        await motorcycleRepository.AddAsync(motorcycle);
    }

    public Task UpdateAsync(Motorcycle motorcycle)
    {
        motorcycleRepository.Update(motorcycle);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        return Task.CompletedTask;
    }
}
