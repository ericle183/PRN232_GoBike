using Services.DTOs;

namespace Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<IEnumerable<CustomerListDto>> GetAllAsync();
    Task<PaginatedResult<CustomerListDto>> GetPaginatedAsync(int page, int pageSize, string? search);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<RentalHistoryDto>> GetRentalHistoryAsync(int customerId);
}

public class RentalHistoryDto
{
    public int Id { get; set; }
    public string MotorcyclePlate { get; set; } = string.Empty;
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
