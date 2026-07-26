using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CupSizeService
{
    private readonly ICupSizeRepository _cupSizeRepository;

    public CupSizeService(ICupSizeRepository cupSizeRepository)
    {
        _cupSizeRepository = cupSizeRepository;
    }

    public async Task<IReadOnlyList<CupSize>> GetAllAsync()
    {
        return await _cupSizeRepository.GetAllAsync();
    }

    public async Task UpdateAsync(CupSize cupSize)
    {
        await _cupSizeRepository.UpdateAsync(cupSize);
    }

    public async Task AddAsync(string name, int volumeMl)
    {
        var cupSize = new CupSize
        {
            Name = name,
            VolumeMl = volumeMl,
        };

        await _cupSizeRepository.AddAsync(cupSize);
    }

    public async Task AddAsync(CupSize cupSize)
    {
        await AddAsync(cupSize.Name, cupSize.VolumeMl);
    }

    public async Task DeleteAsync(CupSize cupSize)
    {
        await _cupSizeRepository.DeleteAsync(cupSize);
    }
}
