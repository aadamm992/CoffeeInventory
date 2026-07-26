using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;
    
    public BrandRepository(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<Brand>> GetAllAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        
        return await dbContext.Brands.AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync(Brand brand)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Brands.Update(brand);

        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Brand brand)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.Brands.AddAsync(brand);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Brand brand)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Brands.Remove(brand);

        await dbContext.SaveChangesAsync();
    }
}
