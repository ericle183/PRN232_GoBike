namespace Services.DTOs;

public class MotorcycleTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultDailyRate { get; set; }
    public bool IsActive { get; set; }
}

public class CreateMotorcycleTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal DefaultDailyRate { get; set; }
}
