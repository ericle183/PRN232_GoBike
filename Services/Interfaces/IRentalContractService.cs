using BusinessObjects.Entities;
using Services.DTOs;

namespace Services.Interfaces;

public interface IRentalContractService
{
    Task<List<RentalContract>> GetAllAsync();
    Task<RentalContract?> GetByIdAsync(int id);
    Task<RentalContract> ReserveAsync(ReserveRentalRequestDto request, int? userId);
    Task<RentalContract> RentNowAsync(RentNowRequestDto request, int? userId);
    Task<RentalContract> HandoverAsync(int id, HandoverRentalRequestDto request, int? userId);
    Task<RentalContract> CompleteAsync(int id, CompleteRentalRequestDto request, int? userId);
    Task<RentalContract> CancelAsync(int id, CancelRentalRequestDto request, int? userId);
    Task<RentalContract> MarkNoShowAsync(int id, NoShowRentalRequestDto request, int? userId);
}
