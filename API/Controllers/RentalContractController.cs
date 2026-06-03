using BusinessObjects.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[ApiController]
[Route("api/[controller]")]
public class RentalContractController : ControllerBase
{
    private readonly IRentalContractService rentalContractService;

    public RentalContractController(IRentalContractService rentalContractService)
    {
        this.rentalContractService = rentalContractService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RentalContractListDto>>> GetAll(
        [FromQuery] int? customerId = null,
        [FromQuery] int? motorcycleId = null,
        [FromQuery] BusinessObjects.Enums.RentalStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var data = await rentalContractService.GetAllAsync();

        if (customerId.HasValue)
        {
            data = data.Where(x => x.CustomerId == customerId.Value).ToList();
        }

        if (motorcycleId.HasValue)
        {
            data = data.Where(x => x.MotorcycleId == motorcycleId.Value).ToList();
        }

        if (status.HasValue)
        {
            data = data.Where(x => x.Status == status.Value).ToList();
        }

        if (fromDate.HasValue)
        {
            data = data.Where(x => x.RentalDate >= fromDate.Value).ToList();
        }

        if (toDate.HasValue)
        {
            data = data.Where(x => x.RentalDate <= toDate.Value).ToList();
        }

        var result = data
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new RentalContractListDto
            {
                Id = x.Id,
                CustomerFullName = x.Customer?.FullName,
                MotorcycleLicensePlate = x.Motorcycle?.LicensePlate,
                RentalDate = x.RentalDate,
                ExpectedReturnDate = x.ExpectedReturnDate,
                DailyRate = x.DailyRate,
                TotalAmount = x.TotalAmount,
                Status = (int)x.Status,
                CreatedBy = x.CreatedBy
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RentalContractDetailDto>> GetById(int id)
    {
        var rental = await rentalContractService.GetByIdAsync(id);
        if (rental is null) return NotFound();
        return Ok(new RentalContractDetailDto
        {
            Id = rental.Id,
            CustomerId = rental.CustomerId,
            MotorcycleId = rental.MotorcycleId,
            CustomerFullName = rental.Customer?.FullName,
            MotorcycleLicensePlate = rental.Motorcycle?.LicensePlate,
            RentalDate = rental.RentalDate,
            ExpectedReturnDate = rental.ExpectedReturnDate,
            ActualReturnDate = rental.ActualReturnDate,
            DailyRate = rental.DailyRate,
            TotalAmount = rental.TotalAmount,
            DepositAmount = rental.DepositAmount,
            FinalAmount = rental.FinalAmount,
            Status = (int)rental.Status,
            StartMileage = rental.StartMileage,
            EndMileage = rental.EndMileage,
            Notes = rental.Notes,
            CreatedBy = rental.CreatedBy
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] RentalContractCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var rental = new RentalContract
        {
            CustomerId = dto.CustomerId,
            MotorcycleId = dto.MotorcycleId,
            RentalDate = dto.RentalDate,
            ExpectedReturnDate = dto.ExpectedReturnDate,
            DepositAmount = dto.DepositAmount,
            Notes = dto.Notes,
            CreatedBy = dto.CreatedBy,
            IsActive = true
        };

        await rentalContractService.CreateAsync(rental);
        return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] RentalContractUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Route id and payload id do not match");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var rental = new RentalContract
        {
            Id = dto.Id,
            CustomerId = dto.CustomerId,
            MotorcycleId = dto.MotorcycleId,
            RentalDate = dto.RentalDate,
            ExpectedReturnDate = dto.ExpectedReturnDate,
            ActualReturnDate = dto.ActualReturnDate,
            DepositAmount = dto.DepositAmount,
            Notes = dto.Notes,
            CreatedBy = dto.CreatedBy,
            StartMileage = dto.StartMileage,
            EndMileage = dto.EndMileage,
            Status = dto.Status,
            IsActive = dto.IsActive
        };

        await rentalContractService.UpdateAsync(rental);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await rentalContractService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/activate")]
    public async Task<ActionResult> Activate(int id, [FromQuery] int startMileage)
    {
        await rentalContractService.ActivateAsync(id, startMileage);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/complete")]
    public async Task<ActionResult> Complete(int id, [FromQuery] DateTime actualReturnDate, [FromQuery] int endMileage)
    {
        await rentalContractService.CompleteAsync(id, actualReturnDate, endMileage);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> Cancel(int id)
    {
        await rentalContractService.CancelAsync(id);
        return NoContent();
    }
}
