using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class BrandService
{
    private readonly IBrandRepository _brandRepository;
    
    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IReadOnlyList<Brand>> GetAllAsync()
    {
        return await _brandRepository.GetAllAsync();
    }

    public async Task UpdateAsync(Brand brand)
    {
        await _brandRepository.UpdateAsync(brand);
    }

    public async Task AddAsync(Brand brand)
    {
        await _brandRepository.AddAsync(brand);
    }

    public async Task DeleteAsync(Brand brand)
    {
        await _brandRepository.DeleteAsync(brand);
    }
}
