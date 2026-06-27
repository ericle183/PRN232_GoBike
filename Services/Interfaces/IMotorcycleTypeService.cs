using BusinessObjects.Entities;
using Services.DTOs;

namespace Services.Interfaces;

public interface IMotorcycleTypeService
{
    Task<List<MotorcycleType>> GetAllAsync();
    Task<MotorcycleType?> GetByIdAsync(int id);
    Task<MotorcycleType> CreateAsync(MotorcycleTypeUpsertDto dto);
    Task<MotorcycleType> UpdateAsync(int id, MotorcycleTypeUpsertDto dto);
    Task DeactivateAsync(int id);
    Task ReactivateAsync(int id);
}
