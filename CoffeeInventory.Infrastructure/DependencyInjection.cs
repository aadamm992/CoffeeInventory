using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Repositories;
using CoffeeInventory.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeInventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICoffeeRepository, CoffeeRepository>();
        services.AddSingleton<ICupSizeRepository, CupSizeRepository>();
        services.AddSingleton<ICapsuleTypeRepository, CapsuleTypeRepository>();
        services.AddSingleton<IBrandRepository, BrandRepository>();

        services.AddSingleton<IDatabaseBackupRecoveryService, DatabaseBackupRecoveryService>();

        return services;
    }
}
