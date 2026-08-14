using CoffeeInventory.Application.Services;
using CoffeeInventory.Wpf.States;
using CoffeeInventory.Wpf.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeInventory.Wpf;

public static class DependencyInjection
{
    public static IServiceCollection AddWpf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<InventoryStore>();

        services.AddTransient<MenuBarViewModel>();
        services.AddTransient(service =>
        {
            var duration = configuration.GetValue<int>("Notification:AutoHideDurationSeconds");

            return new NotificationViewModel(
                service.GetRequiredService<NotificationService>(),
                duration);
        });
        services.AddTransient<TransactionViewModel>();
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<ControlsViewModel>();
        services.AddTransient<BrandControlViewModel>();
        services.AddTransient<CoffeeControlViewModel>();
        services.AddTransient<CapsuleTypeControlViewModel>();
        services.AddTransient<CupSizeControlViewModel>();

        services.AddTransient<MainViewModel>();
        services.AddTransient(service => new MainWindow(service.GetRequiredService<MainViewModel>()));

        return services;
    }
}
