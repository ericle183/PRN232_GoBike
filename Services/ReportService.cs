using BusinessObjects;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Services.DTOs;
using Services.Interfaces;

namespace Services;

public class ReportService : IReportService
{
    private readonly AppDbContext context;

    public ReportService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<DashboardSummaryDto> GetDashboardAsync()
    {
        var today = SystemClock.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var tomorrow = today.AddDays(1);

        return new DashboardSummaryDto
        {
            TotalMotorcycles = await context.Motorcycles.CountAsync(x => x.IsActive),
            AvailableMotorcycles = await context.Motorcycles.CountAsync(x => x.IsActive && x.Status == MotorcycleStatus.Available),
            ReservedMotorcycles = await context.Motorcycles.CountAsync(x => x.IsActive && x.Status == MotorcycleStatus.Reserved),
            RentedMotorcycles = await context.Motorcycles.CountAsync(x => x.IsActive && x.Status == MotorcycleStatus.Rented),
            MaintenanceMotorcycles = await context.Motorcycles.CountAsync(x => x.IsActive && x.Status == MotorcycleStatus.Maintenance),
            ActiveContracts = await context.RentalContracts.CountAsync(x => x.Status == RentalStatus.Active),
            CurrentMonthRevenue = await context.RentalContracts
                .Where(x => x.Status == RentalStatus.Completed
                            && x.CompletedAt >= monthStart
                            && x.CompletedAt < tomorrow)
                .SumAsync(x => x.FinalAmount)
        };
    }

    public async Task<RevenueReportDto> GetRevenueAsync(DateTime? fromDate, DateTime? toDate)
    {
        var (from, toExclusive) = NormalizeRange(fromDate, toDate);

        var query = CompletedContractsInRange(from, toExclusive);
        return new RevenueReportDto
        {
            FromDate = from,
            ToDate = toExclusive.AddDays(-1),
            CompletedContractCount = await query.CountAsync(),
            TotalRevenue = await query.SumAsync(x => x.FinalAmount)
        };
    }

    public async Task<List<TopMotorcycleReportDto>> GetTopMotorcyclesAsync(DateTime? fromDate, DateTime? toDate, int top)
    {
        var (from, toExclusive) = NormalizeRange(fromDate, toDate);
        top = top <= 0 ? 5 : Math.Min(top, 20);

        return await CompletedContractsInRange(from, toExclusive)
            .Where(x => x.Motorcycle != null)
            .GroupBy(x => new
            {
                x.MotorcycleId,
                x.Motorcycle!.LicensePlate,
                x.Motorcycle.Brand,
                x.Motorcycle.Model
            })
            .Select(g => new TopMotorcycleReportDto
            {
                MotorcycleId = g.Key.MotorcycleId,
                LicensePlate = g.Key.LicensePlate,
                Brand = g.Key.Brand,
                Model = g.Key.Model,
                RentalCount = g.Count(),
                TotalRentalDays = g.Sum(x => x.RentalDays),
                TotalRevenue = g.Sum(x => x.FinalAmount)
            })
            .OrderByDescending(x => x.RentalCount)
            .ThenByDescending(x => x.TotalRentalDays)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<ContractStatusReportDto>> GetContractStatusAsync()
    {
        var grouped = await context.RentalContracts
            .GroupBy(x => x.Status)
            .Select(g => new ContractStatusReportDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        return Enum.GetValues<RentalStatus>()
            .Select(status => grouped.FirstOrDefault(x => x.Status == status) ?? new ContractStatusReportDto { Status = status, Count = 0 })
            .ToList();
    }

    public async Task<ReportInsightDto> GetInsightsAsync(DateTime? fromDate, DateTime? toDate)
    {
        var revenue = await GetRevenueAsync(fromDate, toDate);
        var status = await GetContractStatusAsync();
        var topMotorcycles = await GetTopMotorcyclesAsync(fromDate, toDate, 3);
        var dashboard = await GetDashboardAsync();

        var completed = status.First(x => x.Status == RentalStatus.Completed).Count;
        var cancelled = status.First(x => x.Status == RentalStatus.Cancelled).Count;
        var noShow = status.First(x => x.Status == RentalStatus.NoShow).Count;
        var openContracts = status.First(x => x.Status == RentalStatus.Reserved).Count + status.First(x => x.Status == RentalStatus.Active).Count;

        var recommendations = new List<string>();
        if (dashboard.MaintenanceMotorcycles > 0)
        {
            recommendations.Add($"Có {dashboard.MaintenanceMotorcycles} xe đang bảo trì; Admin nên ưu tiên hoàn tất bảo trì để tăng số xe có thể cho thuê.");
        }

        if (cancelled + noShow > completed && completed > 0)
        {
            recommendations.Add("Số hợp đồng hủy/no-show đang cao hơn hợp đồng hoàn thành; nên kiểm tra chính sách giữ cọc và xác nhận khách trước ngày nhận xe.");
        }

        if (openContracts > 0)
        {
            recommendations.Add($"Có {openContracts} hợp đồng đang mở; Staff cần theo dõi giao xe và nhận xe đúng hạn.");
        }

        if (topMotorcycles.Count > 0)
        {
            recommendations.Add($"Xe được thuê nhiều nhất là {topMotorcycles[0].LicensePlate}; nên theo dõi bảo trì để tránh quá tải sử dụng.");
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add("Hoạt động hiện tại ổn định; tiếp tục theo dõi doanh thu và trạng thái xe theo tuần.");
        }

        return new ReportInsightDto
        {
            GeneratedAt = SystemClock.Now,
            Summary = $"Trong khoảng {revenue.FromDate:dd/MM/yyyy} - {revenue.ToDate:dd/MM/yyyy}, hệ thống ghi nhận {revenue.CompletedContractCount} hợp đồng hoàn thành với doanh thu {revenue.TotalRevenue:N0} VND.",
            Recommendations = recommendations
        };
    }

    private IQueryable<BusinessObjects.Entities.RentalContract> CompletedContractsInRange(DateTime from, DateTime toExclusive)
        => context.RentalContracts
            .Include(x => x.Motorcycle)
            .Where(x => x.Status == RentalStatus.Completed
                        && x.CompletedAt >= from
                        && x.CompletedAt < toExclusive);

    private static (DateTime From, DateTime ToExclusive) NormalizeRange(DateTime? fromDate, DateTime? toDate)
    {
        var today = SystemClock.Today;
        var from = (fromDate ?? new DateTime(today.Year, today.Month, 1)).Date;
        var to = (toDate ?? today).Date;

        if (to < from)
        {
            throw new InvalidOperationException("ToDate cannot be earlier than FromDate.");
        }

        return (from, to.AddDays(1));
    }
}
