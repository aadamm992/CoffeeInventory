using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CoffeeInventory.Infrastructure.Data;

internal sealed class CoffeeInventoryDesignDbContext : IDesignTimeDbContextFactory<CoffeeInventoryDbContext>
{
    public CoffeeInventoryDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", true, true)
           .AddJsonFile("appsettings.Development.json", true, true)
           .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
           .AddEnvironmentVariables()
           .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration. " +
                "Please set it in appsettings.json, user secrets, or environment variables.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<CoffeeInventoryDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new CoffeeInventoryDbContext(optionsBuilder.Options);
    }
}
