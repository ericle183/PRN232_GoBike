using BusinessObjects;
using Services.DTOs;

namespace Services.Interfaces;

public interface IMotorcycleService
{
    Task<MotorcycleDto?> GetByIdAsync(int id);
    Task<IEnumerable<MotorcycleDto>> GetAllAsync();
    Task<IEnumerable<MotorcycleTypeDto>> GetTypesAsync();
    Task<PaginatedResult<MotorcycleDto>> GetPaginatedAsync(int page, int pageSize, string? search, MotorcycleStatus? status);
    Task<IEnumerable<MotorcycleDto>> GetAvailableAsync();
    Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto dto);
    Task<MotorcycleDto?> UpdateAsync(int id, UpdateMotorcycleDto dto);
    Task<MotorcycleDto?> UpdateStatusAsync(int id, MotorcycleStatus status);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<MotorcycleDto>> GetRecentRentalsAsync(int motorcycleId, int count);
}
