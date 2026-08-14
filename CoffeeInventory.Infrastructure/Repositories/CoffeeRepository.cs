using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure.Repositories;

public class CoffeeRepository : ICoffeeRepository
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;

    public CoffeeRepository(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Coffee?> GetByIdAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Coffee?> GetByNameAsync(string name)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<IReadOnlyList<Coffee>> GetAllAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Coffee>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Coffee>> GetByNamesAsync(IEnumerable<string> names)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .AsNoTracking()
            .Where(c => names.Contains(c.Name))
            .ToListAsync();
    }

    public async Task UpdateAsync(Coffee coffee)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var existingCoffee = await dbContext.Coffees
            .Include(coffee => coffee.Brand)
            .Include(coffee => coffee.CupSizes)
            .Include(coffee => coffee.CapsuleType)
            .FirstOrDefaultAsync(c => c.Id == coffee.Id);

        if (existingCoffee is null)
        {
            throw new InvalidOperationException("Coffee not found.");
        }

        existingCoffee.Name = coffee.Name;
        existingCoffee.IsDecaffeinated = coffee.IsDecaffeinated;
        existingCoffee.BrandId = coffee.BrandId;
        existingCoffee.CapsuleTypeId = coffee.CapsuleTypeId;
        existingCoffee.Quantity = coffee.Quantity;
        existingCoffee.Consumed = coffee.Consumed;

        existingCoffee.CupSizes.Clear();

        var cupSizes = await dbContext.CupSizes
            .Where(cs => coffee.CupSizes.Select(x => x.Id).Contains(cs.Id))
            .ToListAsync();

        foreach (var cupSize in cupSizes)
        {
            existingCoffee.CupSizes.Add(cupSize);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Guid brandId, Coffee coffee, Guid? capsuleTypeId = null, IEnumerable<Guid>? cupSizeIds = null)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        if (capsuleTypeId.HasValue)
        {
            coffee.CapsuleTypeId = capsuleTypeId;
        }

        if (cupSizeIds != null)
        {
            var cupSizes = await dbContext.CupSizes
                .Where(cs => cupSizeIds.Contains(cs.Id))
                .ToListAsync();

            foreach (var cupSize in cupSizes)
            {
                coffee.CupSizes.Add(cupSize);
            }
        }

        dbContext.Coffees.Add(coffee);

        await dbContext.SaveChangesAsync();
    }

    public async Task AddAsync(Coffee coffee)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.Coffees.AddAsync(coffee);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Coffee coffee)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Coffees.Remove(coffee);

        await dbContext.SaveChangesAsync();
    }
}
