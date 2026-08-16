using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure.Repositories;

public class CupSizeRepository : ICupSizeRepository
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;

    public CupSizeRepository(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<CupSize?> GetByNameAsync(string name)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.CupSizes.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IReadOnlyList<CupSize>> GetAllAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.CupSizes.AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync(CupSize cupSize)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.CupSizes.Update(cupSize);

        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(CupSize cupSize)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.CupSizes.AddAsync(cupSize);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(CupSize cupSize)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.CupSizes.Remove(cupSize);

        await dbContext.SaveChangesAsync();
    }
}
