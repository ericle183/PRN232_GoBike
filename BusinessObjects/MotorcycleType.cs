namespace BusinessObjects;

public class MotorcycleType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultDailyRate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
}
