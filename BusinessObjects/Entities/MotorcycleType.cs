using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities;

public class MotorcycleType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DefaultDailyRate { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
}
