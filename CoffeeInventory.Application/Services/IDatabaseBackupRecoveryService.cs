namespace CoffeeInventory.Application.Services;

public interface IDatabaseBackupRecoveryService
{
    Task BackUpToCsvAsync(string path);
    Task RecoveryFromCsvAsync(string path);
}
