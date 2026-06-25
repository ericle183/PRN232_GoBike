using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class MotorcycleTypeUpsertDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Default daily rate must be greater than 0")]
    public decimal DefaultDailyRate { get; set; }

    [Range(1, double.MaxValue, ErrorMessage = "Default deposit amount must be greater than 0")]
    public decimal DefaultDepositAmount { get; set; }
}
