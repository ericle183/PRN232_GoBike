using Repositories;
using BusinessObjects;
using Services.Interfaces;
using Services.DTOs;

namespace Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMotorcycleTypeRepository _motorcycleTypeRepository;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository, IMotorcycleTypeRepository motorcycleTypeRepository)
    {
        _motorcycleRepository = motorcycleRepository;
        _motorcycleTypeRepository = motorcycleTypeRepository;
    }

    public async Task<MotorcycleDto?> GetByIdAsync(int id)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        return motorcycle == null ? null : MapToDto(motorcycle);
    }

    public async Task<IEnumerable<MotorcycleDto>> GetAllAsync()
    {
        var motorcycles = await _motorcycleRepository.GetAllAsync();
        return motorcycles.Select(MapToDto);
    }

    public async Task<IEnumerable<MotorcycleTypeDto>> GetTypesAsync()
    {
        var types = await _motorcycleTypeRepository.GetAllAsync();
        return types.Select(t => new MotorcycleTypeDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            DefaultDailyRate = t.DefaultDailyRate,
            IsActive = t.IsActive
        });
    }

    public async Task<PaginatedResult<MotorcycleDto>> GetPaginatedAsync(int page, int pageSize, string? search, MotorcycleStatus? status)
    {
        var motorcycles = await _motorcycleRepository.SearchAsync(search, status, page, pageSize);
        var totalCount = await _motorcycleRepository.GetTotalCountAsync(search, status);

        return PaginatedResult<MotorcycleDto>.Create(
            motorcycles.Select(MapToDto),
            page, pageSize, totalCount);
    }

    public async Task<IEnumerable<MotorcycleDto>> GetAvailableAsync()
    {
        var motorcycles = await _motorcycleRepository.GetAvailableMotorcyclesAsync();
        return motorcycles.Select(MapToDto);
    }

    public async Task<MotorcycleDto> CreateAsync(CreateMotorcycleDto dto)
    {
        var motorcycle = new Motorcycle
        {
            LicensePlate = dto.LicensePlate,
            Brand = dto.Brand,
            Model = dto.Model,
            VehicleTypeId = dto.VehicleTypeId,
            DailyRate = dto.DailyRate,
            Color = dto.Color,
            Mileage = dto.Mileage,
            RegistrationNo = dto.RegistrationNo,
            Status = MotorcycleStatus.Available,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _motorcycleRepository.AddAsync(motorcycle);
        return MapToDto(motorcycle);
    }

    public async Task<MotorcycleDto?> UpdateAsync(int id, UpdateMotorcycleDto dto)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null) return null;

        motorcycle.Brand = dto.Brand;
        motorcycle.Model = dto.Model;
        motorcycle.VehicleTypeId = dto.VehicleTypeId;
        motorcycle.DailyRate = dto.DailyRate;
        motorcycle.Color = dto.Color;
        motorcycle.Mileage = dto.Mileage;
        motorcycle.RegistrationNo = dto.RegistrationNo;
        motorcycle.UpdatedAt = DateTime.UtcNow;

        await _motorcycleRepository.UpdateAsync(motorcycle);
        return MapToDto(motorcycle);
    }

    public async Task<MotorcycleDto?> UpdateStatusAsync(int id, MotorcycleStatus status)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null) return null;

        motorcycle.Status = status;
        motorcycle.UpdatedAt = DateTime.UtcNow;
        await _motorcycleRepository.UpdateAsync(motorcycle);
        return MapToDto(motorcycle);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
        if (motorcycle == null) return false;

        motorcycle.IsActive = false;
        motorcycle.UpdatedAt = DateTime.UtcNow;
        await _motorcycleRepository.UpdateAsync(motorcycle);
        return true;
    }

    public async Task<IEnumerable<MotorcycleDto>> GetRecentRentalsAsync(int motorcycleId, int count)
    {
        var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
        if (motorcycle == null) return [];

        return motorcycle.RentalContracts
            .OrderByDescending(r => r.RentalDate)
            .Take(count)
            .Select(_ => MapToDto(motorcycle));
    }

    private static MotorcycleDto MapToDto(Motorcycle m) => new()
    {
        Id = m.Id,
        LicensePlate = m.LicensePlate,
        Brand = m.Brand,
        Model = m.Model,
        VehicleTypeId = m.VehicleTypeId,
        VehicleTypeName = m.VehicleType?.Name,
        Status = m.Status,
        DailyRate = m.DailyRate,
        Color = m.Color,
        Mileage = m.Mileage,
        RegistrationNo = m.RegistrationNo,
        IsActive = m.IsActive,
        CreatedAt = m.CreatedAt,
        UpdatedAt = m.UpdatedAt
    };
}
