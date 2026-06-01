using System.ComponentModel.DataAnnotations;

namespace Services.DTOs;

public class CreateRentalDto
{
    [Required(ErrorMessage = "Customer is required")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Motorcycle is required")]
    public int MotorcycleId { get; set; }

    [Required(ErrorMessage = "Rental date is required")]
    public DateTime RentalDate { get; set; }

    [Required(ErrorMessage = "Expected return date is required")]
    public DateTime ExpectedReturnDate { get; set; }

    public decimal DepositAmount { get; set; } = 0;
    public string? Notes { get; set; }
}

public class ActivateRentalDto
{
    [Required(ErrorMessage = "Start mileage is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Start mileage must be non-negative")]
    public int StartMileage { get; set; }
}

public class CompleteRentalDto
{
    [Required(ErrorMessage = "Actual return date is required")]
    public DateTime ActualReturnDate { get; set; }

    [Required(ErrorMessage = "End mileage is required")]
    [Range(0, int.MaxValue, ErrorMessage = "End mileage must be non-negative")]
    public int EndMileage { get; set; }
}

public class CancelRentalDto
{
    public string? Reason { get; set; }
}
