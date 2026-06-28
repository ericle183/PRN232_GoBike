using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public class Motorcycle
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    public int VehicleTypeId { get; set; }

    public MotorcycleStatus Status { get; set; } = MotorcycleStatus.Available;

    [Required]
    [MaxLength(30)]
    public string Color { get; set; } = string.Empty;

    public int Mileage { get; set; } = 0;

    [MaxLength(20)]
    public string? RegistrationNo { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(VehicleTypeId))]
    public MotorcycleType? VehicleType { get; set; }

    public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
}
