using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal abstract class CommandBase
{
    public abstract Command Build();
}
