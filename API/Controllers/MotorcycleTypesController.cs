using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers;

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
        return Ok(types);
    }
}
