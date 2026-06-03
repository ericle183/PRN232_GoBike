using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotorcycleController : ControllerBase
{
    private readonly IMotorcycleService motorcycleService;

    public MotorcycleController(IMotorcycleService motorcycleService)
    {
        this.motorcycleService = motorcycleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] BusinessObjects.Enums.MotorcycleStatus? status = null)
    {
        var data = await motorcycleService.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();
            data = data.Where(x =>
                x.LicensePlate.ToLower().Contains(search) ||
                x.Brand.ToLower().Contains(search) ||
                x.Model.ToLower().Contains(search)).ToList();
        }

        if (status.HasValue)
        {
            data = data.Where(x => x.Status == status.Value).ToList();
        }

        return Ok(data);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> GetAvailable()
    {
        var data = await motorcycleService.GetAvailableAsync();
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Motorcycle>> GetById(int id)
    {
        var motorcycle = await motorcycleService.GetByIdAsync(id);
        return motorcycle is null ? NotFound() : Ok(motorcycle);
    }
}
