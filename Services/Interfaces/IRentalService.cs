using BusinessObjects;
using Services.DTOs;

namespace Services.Interfaces;

public interface IRentalService
{
    Task<RentalDto?> GetByIdAsync(int id);
    Task<IEnumerable<RentalDto>> GetAllAsync();
    Task<PaginatedResult<RentalDto>> GetPaginatedAsync(
        int page,
        int pageSize,
        RentalStatus? status,
        int? customerId,
        int? motorcycleId,
        DateTime? dateFrom,
        DateTime? dateTo);
    Task<RentalDto> CreateAsync(CreateRentalDto dto, string createdBy);
    Task<RentalDto?> ActivateAsync(int id, ActivateRentalDto dto);
    Task<RentalDto?> CompleteAsync(int id, CompleteRentalDto dto);
    Task<RentalDto?> CancelAsync(int id, string? reason);
}
