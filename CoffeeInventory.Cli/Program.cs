using CoffeeInventory.Application.Services;
using CoffeeInventory.Cli.Commands;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure;
using CoffeeInventory.Infrastructure.Data;
using CoffeeInventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.Reflection;

namespace CoffeeInventory.Cli;

internal abstract class Program
{
    public static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();


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

        services.AddDbContextFactory<CoffeeInventoryDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICoffeeRepository, CoffeeRepository>();
        services.AddScoped<CoffeeService>();
        services.AddScoped<IDatabaseBackupRecoveryService, DatabaseBackupRecoveryService>();

        services.AddTransient<AddCommand>();
        services.AddTransient<BackupInventoryCommand>();
        services.AddTransient<ConsumedCommand>();
        services.AddTransient<InventoryCommand>();
        services.AddTransient<RecoveryInventoryCommand>();
        services.AddTransient<ReplenishmentCommand>();
        services.AddTransient<TotalCommand>();

        await using var provider = services.BuildServiceProvider();

        var rootCommand = new RootCommand("Nespresso Inventory CLI app")
        {
            Aliases = { "nespresso", "nespresso-inventory" },
        };

        var commands = new List<Command>
         {
             provider.GetRequiredService<AddCommand>().Build(),
             provider.GetRequiredService<BackupInventoryCommand>().Build(),
             provider.GetRequiredService<ConsumedCommand>().Build(),
             provider.GetRequiredService<InventoryCommand>().Build(),
             provider.GetRequiredService<RecoveryInventoryCommand>().Build(),
             provider.GetRequiredService<ReplenishmentCommand>().Build(),
             provider.GetRequiredService<TotalCommand>().Build(),
         };

        foreach (var command in commands)
        {
            rootCommand.Subcommands.Add(command);
        }

        return await rootCommand.Parse(args).InvokeAsync();
    }
}