using CoffeeInventory.Application.Services;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class BackupInventoryCommand(IDatabaseBackupRecoveryService databaseBackupRecoveryService)
{
    public Command Build()
    {
        var command = new Command("Backup", "Back up the current state of inventory")
        {
            Aliases = { "b", "backup" },
        };
        
        command.SetAction(async _ =>
        {
            await databaseBackupRecoveryService.BackupAsync();
        });
        
        return command;
    }
}
