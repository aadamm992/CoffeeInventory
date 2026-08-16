using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class BrandService : ServiceBase<Brand>
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository) : base(brandRepository)
    {
        _brandRepository = brandRepository;
    }
}
