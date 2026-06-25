using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService service;

    public ReportsController(IReportService service)
    {
        this.service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await service.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var result = await service.GetRevenueAsync(fromDate, toDate);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("top-motorcycles")]
    public async Task<IActionResult> GetTopMotorcycles(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int top = 5)
    {
        try
        {
            var result = await service.GetTopMotorcyclesAsync(fromDate, toDate, top);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("contract-status")]
    public async Task<IActionResult> GetContractStatus()
    {
        var result = await service.GetContractStatusAsync();
        return Ok(result);
    }

    [HttpGet("insights")]
    public async Task<IActionResult> GetInsights([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        try
        {
            var result = await service.GetInsightsAsync(fromDate, toDate);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
