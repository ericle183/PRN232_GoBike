using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[ApiController]
[Route("api/[controller]")]
public class MotorcycleTypesController : ControllerBase
{
    private readonly IMotorcycleTypeService _service;

    public MotorcycleTypesController(IMotorcycleTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var types = await _service.GetAllAsync();
        var dtos = types.Select(t => new MotorcycleTypeDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            DefaultDailyRate = t.DefaultDailyRate
        }).ToList();
        return Ok(dtos);
    }
}
