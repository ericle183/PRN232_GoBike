namespace Services.DTOs;

public class RentalContractListDto
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

public class RentalContractDetailDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int MotorcycleId { get; set; }
    public string? CustomerFullName { get; set; }
    public string? MotorcycleLicensePlate { get; set; }
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
}
