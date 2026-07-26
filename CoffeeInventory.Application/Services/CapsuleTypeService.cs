using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CapsuleTypeService
{
    private readonly ICapsuleTypeRepository _capsuleTypeRepository;

    public CapsuleTypeService(ICapsuleTypeRepository capsuleTypeRepository)
    {
        _capsuleTypeRepository = capsuleTypeRepository;
    }

    public async Task<IReadOnlyList<CapsuleType>> GetAllAsync()
    {
        return await _capsuleTypeRepository.GetAllAsync();
    }

    public async Task UpdateAsync(CapsuleType capsuleType)
    {
        await _capsuleTypeRepository.UpdateAsync(capsuleType);
    }

    public async Task AddAsync(CapsuleType capsuleType)
    {
        await _capsuleTypeRepository.AddAsync(capsuleType);
    }

    public async Task DeleteAsync(CapsuleType capsuleType)
    {
        await _capsuleTypeRepository.DeleteAsync(capsuleType);
    }
}
