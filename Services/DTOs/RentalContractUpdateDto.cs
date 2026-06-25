using System.ComponentModel.DataAnnotations;
using BusinessObjects.Enums;

namespace Services.DTOs;

public class RentalContractUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int MotorcycleId { get; set; }

    [Required]
    public DateTime RentalDate { get; set; }

    [Required]
    public DateTime ExpectedReturnDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal DepositAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public int? StartMileage { get; set; }

    public int? EndMileage { get; set; }

    public RentalStatus Status { get; set; }

    public bool IsActive { get; set; } = true;
}
