using BusinessObjects;

namespace Services.DTOs;

public class MotorcycleDto
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public string? VehicleTypeName { get; set; }
    public MotorcycleStatus Status { get; set; }
    public decimal DailyRate { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string? RegistrationNo { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
