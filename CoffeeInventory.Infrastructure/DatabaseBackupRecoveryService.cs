using CoffeeInventory.Application.Services;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure;

public class DatabaseBackupRecoveryService : IDatabaseBackupRecoveryService
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;

    public DatabaseBackupRecoveryService(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task BackupAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var filePath = $@"C:\DatabaseBackup\CoffeeInventory{DateTime.Now:yyyyMMddHHmmss}.bak";

        await dbContext.Database.ExecuteSqlRawAsync(
            "BACKUP DATABASE [CoffeeInventory] TO DISK = @filePath WITH INIT",
            new SqlParameter("@filePath", filePath));
    }
}
