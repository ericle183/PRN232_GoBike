using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class UpdateMotorcycleRequest
{
    [RegularExpression(@"^[0-9]{2}[A-Z]{1,2}-[0-9]{4,5}$",
        ErrorMessage = "Invalid VN license plate format (e.g., 51A-12345)")]
    public string? LicensePlate { get; set; }

    [MaxLength(50)]
    public string? Brand { get; set; }

    [MaxLength(50)]
    public string? Model { get; set; }

    public int? VehicleTypeId { get; set; }

    [MaxLength(30)]
    public string? Color { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
    public int? Mileage { get; set; }

    [MaxLength(20)]
    public string? RegistrationNo { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }
}
