using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CapsuleTypeService : ServiceBase<CapsuleType>
{
    private readonly ICapsuleTypeRepository _capsuleTypeRepository;

    public CapsuleTypeService(ICapsuleTypeRepository capsuleTypeRepository) : base(capsuleTypeRepository)
    {
        _capsuleTypeRepository = capsuleTypeRepository;
    }
}
