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
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyPrice { get; set; }

    public int RentalDays { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DepositAmount { get; set; } = 0;

    public int LateDays { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LateFee { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DamageFee { get; set; }

    [MaxLength(500)]
    public string? DamageDescription { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherFee { get; set; }

    [MaxLength(500)]
    public string? OtherFeeDescription { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [MaxLength(500)]
    public string? DiscountReason { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RemainingAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AdditionalPaymentAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RefundAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CancellationFee { get; set; }

    public RentalStatus Status { get; set; } = RentalStatus.Reserved;

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? UpdatedByUserId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int? CompletedByUserId { get; set; }

    public DateTime? CancelledAt { get; set; }

    public int? CancelledByUserId { get; set; }

    [MaxLength(500)]
    public string? CancellationReason { get; set; }

    public DateTime? NoShowAt { get; set; }

    public int? NoShowByUserId { get; set; }

    [MaxLength(500)]
    public string? NoShowReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public DateTime RentalDate
    {
        get => StartDate;
        set => StartDate = value;
    }

    [NotMapped]
    public DateTime ExpectedReturnDate
    {
        get => EndDate;
        set => EndDate = value;
    }

    [NotMapped]
    public decimal DailyRate
    {
        get => DailyPrice;
        set => DailyPrice = value;
    }

    [NotMapped]
    public int? StartMileage { get; set; }

    [NotMapped]
    public int? EndMileage { get; set; }

    [NotMapped]
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    [ForeignKey(nameof(MotorcycleId))]
    public Motorcycle? Motorcycle { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }

    [ForeignKey(nameof(UpdatedByUserId))]
    public User? UpdatedByUser { get; set; }

    [ForeignKey(nameof(CompletedByUserId))]
    public User? CompletedByUser { get; set; }

    [ForeignKey(nameof(CancelledByUserId))]
    public User? CancelledByUser { get; set; }

    [ForeignKey(nameof(NoShowByUserId))]
    public User? NoShowByUser { get; set; }

    public ICollection<RentalInspection> Inspections { get; set; } = new List<RentalInspection>();

    public ICollection<RentalPayment> Payments { get; set; } = new List<RentalPayment>();

    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
}
