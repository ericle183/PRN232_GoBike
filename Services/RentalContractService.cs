using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class RentalContractService : IRentalContractService
{
    private readonly IRentalContractRepository rentalContractRepository;

    public RentalContractService(IRentalContractRepository rentalContractRepository)
    {
        this.rentalContractRepository = rentalContractRepository;
    }

    public Task<List<RentalContract>> GetAllAsync()
        => rentalContractRepository.GetAllAsync();

    public Task<RentalContract?> GetByIdAsync(int id)
        => rentalContractRepository.GetByIdAsync(id);

    public async Task CreateAsync(RentalContract rentalContract)
    {
        await rentalContractRepository.AddAsync(rentalContract);
    }

    public Task UpdateAsync(RentalContract rentalContract)
    {
        rentalContractRepository.Update(rentalContract);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        return Task.CompletedTask;
    }
}
