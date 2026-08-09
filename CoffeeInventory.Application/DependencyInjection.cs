using CoffeeInventory.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeInventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) 
    {
        services.AddSingleton<CoffeeService>();
        services.AddSingleton<CupSizeService>();
        services.AddSingleton<CapsuleTypeService>();
        services.AddSingleton<BrandService>();

        services.AddSingleton<NotificationService>();

        return services;
    }

}
