using System.ComponentModel.DataAnnotations;
using BusinessObjects;

namespace Services.DTOs;

public class CreateMotorcycleDto
{
    [Required(ErrorMessage = "License plate is required")]
    [RegularExpression(@"^[0-9]{2}[A-Z]{1,2}-[0-9]{4,5}$", ErrorMessage = "Invalid VN license plate format (e.g., 51A-12345)")]
    public string LicensePlate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Brand is required")]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vehicle type is required")]
    public int VehicleTypeId { get; set; }

    [Required(ErrorMessage = "Daily rate is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Daily rate must be positive")]
    public decimal DailyRate { get; set; }

    [Required(ErrorMessage = "Color is required")]
    public string Color { get; set; } = string.Empty;

    public int Mileage { get; set; } = 0;
    public string? RegistrationNo { get; set; }
}

public class UpdateMotorcycleDto
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public decimal DailyRate { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string? RegistrationNo { get; set; }
}

public class UpdateMotorcycleStatusDto
{
    public MotorcycleStatus Status { get; set; }
}
