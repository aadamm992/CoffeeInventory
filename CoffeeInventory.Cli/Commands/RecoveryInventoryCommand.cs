using CoffeeInventory.Application.Services;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class RecoveryInventoryCommand(IDatabaseBackupRecoveryService databaseBackupRecoveryService)
{
    public Command Build()
    {
        var command = new Command("Recovery", "Recovery the set to a previous state")
        {
            Aliases = { "recovery" },
        };

        var backupFilePath = new Option<string>("--path", "-p")
        {
            Description = "Back up file path",
            Required = true,
        };

        command.Options.Add(backupFilePath);

        command.SetAction(async parseResult =>
        {
            var path = parseResult.GetValue(backupFilePath);

            await databaseBackupRecoveryService.RecoveryFromCsvAsync(path!);
        });

        return command;
    }
}