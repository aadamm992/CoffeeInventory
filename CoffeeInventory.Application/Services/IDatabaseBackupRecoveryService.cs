namespace CoffeeInventory.Application.Services;

public interface IDatabaseBackupRecoveryService
{
    Task BackupAsync();
}
