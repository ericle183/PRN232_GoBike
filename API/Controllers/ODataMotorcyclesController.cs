using BusinessObjects.Entities;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
public class ODataMotorcyclesController : ODataController
{
    private readonly AppDbContext _context;

    public ODataMotorcyclesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<Motorcycle> Get()
    {
        return _context.Motorcycles.Include(m => m.VehicleType);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    public IQueryable<Motorcycle> Get(int key)
    {
        return _context.Motorcycles.Include(m => m.VehicleType).Where(m => m.Id == key);
    }
}
