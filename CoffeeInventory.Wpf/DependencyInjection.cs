using CoffeeInventory.Wpf.States;
using CoffeeInventory.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeInventory.Wpf;

public static class DependencyInjection
{
    public static IServiceCollection AddWpf(this IServiceCollection services)
    {
        services.AddSingleton<InventoryStore>();

        services.AddSingleton<MenuBarViewModel>();
        services.AddTransient<NotificationViewModel>();
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
