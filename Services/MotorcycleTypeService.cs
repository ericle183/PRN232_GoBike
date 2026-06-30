using BusinessObjects;
using BusinessObjects.Entities;
using Repositories;
using Services.DTOs;
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

    public Task<MotorcycleType?> GetByIdAsync(int id)
        => motorcycleTypeRepository.GetByIdAsync(id);

    public async Task<MotorcycleType> CreateAsync(MotorcycleTypeUpsertDto dto)
    {
        Validate(dto);

        var name = dto.Name.Trim();
        if (await motorcycleTypeRepository.ExistsByNameAsync(name))
        {
            throw new InvalidOperationException("Motorcycle type name already exists.");
        }

        var type = new MotorcycleType
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            DefaultDailyRate = dto.DefaultDailyRate,
            DefaultDepositAmount = dto.DefaultDepositAmount,
            IsActive = true,
            CreatedAt = SystemClock.Now
        };

        await motorcycleTypeRepository.AddAsync(type);
        return type;
    }

    public async Task<MotorcycleType> UpdateAsync(int id, MotorcycleTypeUpsertDto dto)
    {
        Validate(dto);

        var type = await motorcycleTypeRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Motorcycle type with ID {id} not found.");

        var name = dto.Name.Trim();
        if (await motorcycleTypeRepository.ExistsByNameAsync(name, id))
        {
            throw new InvalidOperationException("Motorcycle type name already exists.");
        }

        type.Name = name;
        type.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        type.DefaultDailyRate = dto.DefaultDailyRate;
        type.DefaultDepositAmount = dto.DefaultDepositAmount;
        type.UpdatedAt = SystemClock.Now;

        motorcycleTypeRepository.Update(type);
        return type;
    }

    public async Task DeactivateAsync(int id)
    {
        var type = await motorcycleTypeRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Motorcycle type with ID {id} not found.");

        if (await motorcycleTypeRepository.HasActiveMotorcyclesAsync(id))
        {
            throw new InvalidOperationException("Cannot deactivate a motorcycle type that is used by active motorcycles.");
        }

        type.IsActive = false;
        type.UpdatedAt = SystemClock.Now;
        motorcycleTypeRepository.Update(type);
    }

    public async Task ReactivateAsync(int id)
    {
        var type = await motorcycleTypeRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Motorcycle type with ID {id} not found.");

        type.IsActive = true;
        type.UpdatedAt = SystemClock.Now;
        motorcycleTypeRepository.Update(type);
    }

    private static void Validate(MotorcycleTypeUpsertDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new InvalidOperationException("Name is required.");
        }

        if (dto.DefaultDailyRate <= 0)
        {
            throw new InvalidOperationException("Default daily rate must be greater than 0.");
        }

        if (dto.DefaultDepositAmount <= 0)
        {
            throw new InvalidOperationException("Default deposit amount must be greater than 0.");
        }
    }
}
