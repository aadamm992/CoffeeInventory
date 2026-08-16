using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CupSizeService : ServiceBase<CupSize>
{
    private readonly ICupSizeRepository _cupSizeRepository;

    public CupSizeService(ICupSizeRepository cupSizeRepository) : base(cupSizeRepository)
    {
        _cupSizeRepository = cupSizeRepository;
    }
}
