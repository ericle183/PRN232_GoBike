using BusinessObjects.Entities;
using Repositories;
using Services.Interfaces;

namespace Services;

public class MotorcycleTypeService : IMotorcycleTypeService
{
    private readonly IMotorcycleTypeRepository motorcycleTypeRepository;

    public MotorcycleTypeService(IMotorcycleTypeRepository motorcycleTypeRepository)
    {
        this.motorcycleTypeRepository = motorcycleTypeRepository;
    }

    public Task<List<MotorcycleType>> GetAllAsync()
        => motorcycleTypeRepository.GetAllAsync();
}
