namespace Services.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string CCCD { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string DriverLicenseNo { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CustomerListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string MaskedCCCD { get; set; } = string.Empty;
    public string MaskedPhone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
