namespace WebUI.Pages.Rentals;

public class RentalListItem
{
    public int Id { get; set; }
    public string? CustomerFullName { get; set; }
    public string? MotorcycleLicensePlate { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string? CreatedBy { get; set; }
}

public class RentalDetail
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int MotorcycleId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DepositAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public int Status { get; set; }
    public int? StartMileage { get; set; }
    public int? EndMileage { get; set; }
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public string? CustomerFullName { get; set; }
    public string? MotorcycleLicensePlate { get; set; }
}

public class CustomerOption
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
}

public class MotorcycleOption
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public decimal DailyRate { get; set; }
    public int Mileage { get; set; }
}

public class AvailableMotorcycle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public decimal DailyRate { get; set; }
    public int Mileage { get; set; }
}

public class RentalCreateForm
{
    public int CustomerId { get; set; }
    public int MotorcycleId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public decimal DepositAmount { get; set; }
    public string? Notes { get; set; }
}

public class RentalCreatedResult
{
    public int Id { get; set; }
}
