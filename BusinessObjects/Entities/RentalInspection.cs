using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public class RentalInspection
{
    [Key]
    public int Id { get; set; }

    public int RentalContractId { get; set; }

    public InspectionType InspectionType { get; set; }

    [Range(0, int.MaxValue)]
    public int Mileage { get; set; }

    [Required]
    [MaxLength(50)]
    public string FuelLevel { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string VehicleCondition { get; set; } = string.Empty;

    public bool HasDamage { get; set; }

    [MaxLength(500)]
    public string? DamageDescription { get; set; }

    [MaxLength(500)]
    public string? AccessoriesNote { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(RentalContractId))]
    public RentalContract? RentalContract { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }
}
