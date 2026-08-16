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
using System.Diagnostics;
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
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICapsuleTypeRepository, CapsuleTypeRepository>();
        services.AddScoped<ICupSizeRepository, CupSizeRepository>();

        services.AddScoped<CoffeeService>();
        services.AddScoped<BrandService>();
        services.AddScoped<CapsuleTypeService>();
        services.AddScoped<CupSizeService>();

        services.AddScoped<IDatabaseBackupRecoveryService, DatabaseBackupRecoveryService>();

        services.AddTransient<AddCoffeeCommand>();
        services.AddTransient<BackupInventoryCommand>();
        services.AddTransient<ConsumedCommand>();
        services.AddTransient<InventoryCommand>();
        services.AddTransient<ReplenishmentCommand>();
        services.AddTransient<TotalCommand>();
        services.AddTransient<ListEntity>();

        await using var provider = services.BuildServiceProvider();

        var rootCommand = new RootCommand("Nespresso Inventory CLI app")
        {
            Aliases = { "nespresso", "nespresso-inventory" },
        };

        var commands = new List<CommandBase>
         {
             provider.GetRequiredService<AddCoffeeCommand>(),
             provider.GetRequiredService<BackupInventoryCommand>(),
             provider.GetRequiredService<ConsumedCommand>(),
             provider.GetRequiredService<InventoryCommand>(),
             provider.GetRequiredService<ReplenishmentCommand>(),
             provider.GetRequiredService<TotalCommand>(),
             provider.GetRequiredService<ListEntity>(),
         };

        foreach (var command in commands)
        {
            rootCommand.Subcommands.Add(command.Build());
        }

        try
        {
            return await rootCommand.Parse(args).InvokeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\e[31m{ex.Message}\e[0m");
            Debug.WriteLine(ex);
            return 1;
        }
    }
}
