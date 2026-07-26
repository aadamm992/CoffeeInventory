using CoffeeInventory.Application.Services;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class BackupInventoryCommand(IDatabaseBackupRecoveryService databaseBackupRecoveryService)
{
    private readonly string _backupFolderPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CoffeeInventory",
            "backup");
    
    public Command Build()
    {
        var command = new Command("Backup", "Back up the current state of inventory")
        {
            Aliases = { "b", "backup" },
        };
        
        command.SetAction(async _ =>
        {
            await databaseBackupRecoveryService.BackUpToCsvAsync(_backupFolderPath);
        });
        
        return command;
    }
}
