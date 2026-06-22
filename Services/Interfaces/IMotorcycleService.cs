using BusinessObjects.Enums;
using Services.DTOs;

namespace Services.Interfaces;

public interface IMotorcycleService
{
    Task<PaginatedResult<MotorcycleDto>> GetPaginatedAsync(
        string? search,
        MotorcycleStatus? status,
        decimal? minPrice,
        decimal? maxPrice,
        int page = 1,
        int pageSize = 10);
    Task<MotorcycleDetailDto?> GetDetailAsync(int id);
    Task<MotorcycleDto?> GetByIdAsync(int id);
    Task<List<MotorcycleDto>> GetAvailableAsync();
    Task<MotorcycleDto> CreateAsync(CreateMotorcycleRequest request);
    Task<MotorcycleDto> UpdateAsync(int id, UpdateMotorcycleRequest request, string userRole);
    Task SoftDeleteAsync(int id);
    Task UpdateStatusAsync(int id, MotorcycleStatus newStatus);
    Task UpdateMileageAsync(int id, int newMileage);
}
