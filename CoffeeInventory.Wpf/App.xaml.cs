using CoffeeInventory.Application;
using CoffeeInventory.Infrastructure;
using CoffeeInventory.Infrastructure.Data;
using CoffeeInventory.Wpf.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CoffeeInventory.Wpf;

public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
           .UseSerilog((context, services, loggerConfiguration) =>
           {
               loggerConfiguration
                   .ReadFrom.Configuration(context.Configuration)
                   .ReadFrom.Services(services);
           })
           .ConfigureAppConfiguration((context, builder) =>
           {
               builder.SetBasePath(Directory.GetCurrentDirectory());
               builder.AddJsonFile("appsettings.json", true, true);
               builder.AddJsonFile("appsettings.Development.json", true, true);
               builder.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
               builder.AddEnvironmentVariables();
           })
           .ConfigureServices((context, services) =>
           {
               var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

               services.AddDbContextFactory<CoffeeInventoryDbContext>(options =>
                   options.UseSqlServer(connectionString));

               services.AddInfrastructure();
               services.AddApplication();
               services.AddWpf(context.Configuration);
           })
           .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        Log.Information("Application started.");

        var inventoryStore = _host.Services.GetRequiredService<InventoryStore>();
        await inventoryStore.InitializeAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        await Log.CloseAndFlushAsync();

        base.OnExit(e);
    }
}