using Repositories;
using BusinessObjects;
using Services.Interfaces;
using Services.DTOs;

namespace Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<IEnumerable<CustomerListDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(MapToListDto);
    }

    public async Task<PaginatedResult<CustomerListDto>> GetPaginatedAsync(int page, int pageSize, string? search)
    {
        var customers = await _customerRepository.SearchAsync(search, page, pageSize);
        var totalCount = await _customerRepository.GetTotalCountAsync(search);

        return PaginatedResult<CustomerListDto>.Create(
            customers.Select(MapToListDto),
            page, pageSize, totalCount);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            FullName = dto.FullName,
            CCCD = dto.CCCD,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            DriverLicenseNo = dto.DriverLicenseNo,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _customerRepository.AddAsync(customer);
        return MapToDto(customer);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        customer.FullName = dto.FullName;
        customer.Email = dto.Email;
        customer.Address = dto.Address;
        if (dto.DateOfBirth.HasValue) customer.DateOfBirth = dto.DateOfBirth.Value;
        if (!string.IsNullOrEmpty(dto.DriverLicenseNo)) customer.DriverLicenseNo = dto.DriverLicenseNo;
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);
        return MapToDto(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hasHistory = await _customerRepository.HasRentalHistoryAsync(id);
        if (hasHistory) return false;

        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;

        customer.IsActive = false;
        customer.UpdatedAt = DateTime.UtcNow;
        await _customerRepository.UpdateAsync(customer);
        return true;
    }

    public async Task<IEnumerable<RentalHistoryDto>> GetRentalHistoryAsync(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) return [];

        return customer.RentalContracts
            .OrderByDescending(r => r.RentalDate)
            .Take(10)
            .Select(r => new RentalHistoryDto
            {
                Id = r.Id,
                MotorcyclePlate = r.Motorcycle?.LicensePlate ?? string.Empty,
                RentalDate = r.RentalDate,
                ReturnDate = r.ActualReturnDate,
                Status = r.Status.ToString(),
                TotalAmount = r.FinalAmount > 0 ? r.FinalAmount : r.TotalAmount
            });
    }

    private static CustomerDto MapToDto(Customer c) => new()
    {
        Id = c.Id,
        FullName = c.FullName,
        CCCD = c.CCCD,
        PhoneNumber = c.PhoneNumber,
        Email = c.Email,
        Address = c.Address,
        DateOfBirth = c.DateOfBirth,
        Age = c.Age,
        DriverLicenseNo = c.DriverLicenseNo,
        IsActive = c.IsActive,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };

    private static CustomerListDto MapToListDto(Customer c) => new()
    {
        Id = c.Id,
        FullName = c.FullName,
        MaskedCCCD = MaskString(c.CCCD),
        MaskedPhone = MaskString(c.PhoneNumber),
        CreatedAt = c.CreatedAt
    };

    private static string MaskString(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 6) return value;
        return value[..4] + new string('*', value.Length - 6) + value[^2..];
    }
}
