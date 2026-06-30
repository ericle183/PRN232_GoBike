using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObjects;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public class MaintenanceRecord
{
    [Key]
    public int Id { get; set; }

    public int MotorcycleId { get; set; }

    public int? RentalContractId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RepairCost { get; set; }

    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

    public DateTime StartDate { get; set; } = SystemClock.Today;

    public DateTime? EndDate { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = SystemClock.Now;

    public int? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(MotorcycleId))]
    public Motorcycle? Motorcycle { get; set; }

    [ForeignKey(nameof(RentalContractId))]
    public RentalContract? RentalContract { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }

    [ForeignKey(nameof(UpdatedByUserId))]
    public User? UpdatedByUser { get; set; }
}
