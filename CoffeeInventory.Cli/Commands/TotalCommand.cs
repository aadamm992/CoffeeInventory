using System.CommandLine;
using CoffeeInventory.Application.Services;

namespace CoffeeInventory.Cli.Commands;

internal class TotalCommand(CoffeeService coffeeService)
{
    public Command Build()
    {
        var command = new Command("Total", "Shows the available coffee sum.")
        {
            Aliases = { "t", "total" },
        };

        command.SetAction(async _ =>
        {
            var coffees = await coffeeService.GetAllAsync();
            var total = coffees.Sum(c => c.Quantity);

            Console.WriteLine($"Coffee total sum: {total}");
        });

        return command;
    }
}
