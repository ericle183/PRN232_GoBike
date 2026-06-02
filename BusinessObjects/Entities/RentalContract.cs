using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public class RentalContract
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int MotorcycleId { get; set; }

    [Required]
    public DateTime RentalDate { get; set; }

    [Required]
    public DateTime ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalAmount { get; set; }

    public RentalStatus Status { get; set; } = RentalStatus.Pending;

    public int? StartMileage { get; set; }

    public int? EndMileage { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    [ForeignKey(nameof(MotorcycleId))]
    public Motorcycle? Motorcycle { get; set; }
}
