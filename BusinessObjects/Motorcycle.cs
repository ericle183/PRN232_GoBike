namespace BusinessObjects;

public class Motorcycle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public MotorcycleStatus Status { get; set; } = MotorcycleStatus.Available;
    public decimal DailyRate { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string? RegistrationNo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public MotorcycleType? VehicleType { get; set; }
    public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
}
