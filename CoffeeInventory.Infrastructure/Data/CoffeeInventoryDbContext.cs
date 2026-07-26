using CoffeeInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure.Data;

public sealed class CoffeeInventoryDbContext(DbContextOptions<CoffeeInventoryDbContext> options)
    : DbContext(options)
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Coffee> Coffees { get; set; }
    public DbSet<CupSize> CupSizes { get; set; }
    public DbSet<CapsuleType> CapsuleTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeInventoryDesignDbContext).Assembly);
    }
}
