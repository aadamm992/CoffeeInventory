using System.CommandLine;
using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Enums;

namespace CoffeeInventory.Cli.Commands;

internal class ReplenishmentCommand(CoffeeService coffeeService) : CommandBase
{
    public override Command Build()
    {
        var command = new Command("Replenishment", "Submitted coffee replenishment")
        {
            Aliases = { "r", "replenishment" }
        };

        var namesOptions = new Option<string[]>("--name", "-n")
        {
            Required = true,
            AllowMultipleArgumentsPerToken = true,
            Description = "Coffee Name",
        };

        var quantityOption = new Option<int>("--quantity", "-q")
        {
            Description = "Replenished quantity",
            DefaultValueFactory = _ => 10,
        };

        command.Options.Add(namesOptions);
        command.Options.Add(quantityOption);

        command.SetAction(async parseResult =>
        {
            var names = parseResult.GetValue(namesOptions);
            var quantity = parseResult.GetValue(quantityOption);

            if (names != null)
            {
                await coffeeService.UpdateCoffeesBatchAsync(names, quantity, TransactionType.Replenishment);

                foreach (var name in names)
                {
                    var coffee = await coffeeService.GetByNameAsync(name);

                    Console.WriteLine(coffee is not null
                        ? $"\e[33m{quantity} pieces {coffee.Brand} {coffee.Name} have been added to the stock\e[0m"
                        : "\e[33mCoffee does not exist\e[0m");
                }
            }
        });

        return command;
    }
}