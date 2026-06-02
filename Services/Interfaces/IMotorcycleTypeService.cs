using BusinessObjects.Entities;

namespace Services.Interfaces;

public interface IMotorcycleTypeService
{
    Task<List<MotorcycleType>> GetAllAsync();
}
