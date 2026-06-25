using BusinessObjects.Enums;

namespace Services.DTOs;

public class DashboardSummaryDto
{
    public int TotalMotorcycles { get; set; }
    public int AvailableMotorcycles { get; set; }
    public int ReservedMotorcycles { get; set; }
    public int RentedMotorcycles { get; set; }
    public int MaintenanceMotorcycles { get; set; }
    public int ActiveContracts { get; set; }
    public decimal CurrentMonthRevenue { get; set; }
}

public class RevenueReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int CompletedContractCount { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class TopMotorcycleReportDto
{
    public int MotorcycleId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int RentalCount { get; set; }
    public int TotalRentalDays { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class ContractStatusReportDto
{
    public RentalStatus Status { get; set; }
    public int Count { get; set; }
}

public class ReportInsightDto
{
    public DateTime GeneratedAt { get; set; }
    public string Summary { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = [];
}
