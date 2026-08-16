using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure.Repositories;

public class CapsuleTypeRepository : ICapsuleTypeRepository
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;

    public CapsuleTypeRepository(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<CapsuleType?> GetByNameAsync(string name)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.CapsuleTypes.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IReadOnlyList<CapsuleType>> GetAllAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.CapsuleTypes.AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync(CapsuleType capsuleType)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.CapsuleTypes.Update(capsuleType);

        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(CapsuleType capsuleType)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.CapsuleTypes.AddAsync(capsuleType);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(CapsuleType capsuleType)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.CapsuleTypes.Remove(capsuleType);

        await dbContext.SaveChangesAsync();
    }

}
