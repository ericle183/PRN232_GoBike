using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class CreateMotorcycleRequest
{
    [Required(ErrorMessage = "License plate is required")]
    [RegularExpression(@"^[0-9]{2}[A-Z]{1,2}-[0-9]{4,5}$",
        ErrorMessage = "Invalid VN license plate format (e.g., 51A-12345)")]
    public string LicensePlate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Brand is required")]
    [MaxLength(50)]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required")]
    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vehicle type is required")]
    public int VehicleTypeId { get; set; }

    [Required(ErrorMessage = "Daily rate is required")]
    [Range(1000, 10_000_000, ErrorMessage = "Daily rate must be between 1,000 and 10,000,000 VND")]
    public decimal DailyRate { get; set; }

    [Required(ErrorMessage = "Color is required")]
    [MaxLength(30)]
    public string Color { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
    public int Mileage { get; set; } = 0;

    [MaxLength(20)]
    public string? RegistrationNo { get; set; }
}
