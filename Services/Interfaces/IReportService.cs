using Services.DTOs;

namespace Services.Interfaces;

public interface IReportService
{
    Task<DashboardSummaryDto> GetDashboardAsync();
    Task<RevenueReportDto> GetRevenueAsync(DateTime? fromDate, DateTime? toDate);
    Task<List<TopMotorcycleReportDto>> GetTopMotorcyclesAsync(DateTime? fromDate, DateTime? toDate, int top);
    Task<List<ContractStatusReportDto>> GetContractStatusAsync();
    Task<ReportInsightDto> GetInsightsAsync(DateTime? fromDate, DateTime? toDate);
}
