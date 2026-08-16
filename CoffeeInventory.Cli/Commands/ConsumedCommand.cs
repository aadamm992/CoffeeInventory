using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Enums;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class ConsumedCommand(CoffeeService coffeeService) : CommandBase
{
    public override Command Build()
    {
        var command = new Command("Consumed", "Submitted coffee consumed")
        {
            Aliases = { "c", "consumed" },
        };

        var namesOption = new Option<string[]>("--name", "-n")
        {
            Description = "Coffee name",
            AllowMultipleArgumentsPerToken = true,
        };

        var quantityOption = new Option<int>("--quantity", "-q")
        {
            Description = "Consumed quantity",
            DefaultValueFactory = _ => 1,
        };

        var isRemainingOption = new Option<bool>("--isRemaining", "-i")
        {
            Description = "Submit remaining quantity",
        };

        command.Options.Add(namesOption);
        command.Options.Add(quantityOption);
        command.Options.Add(isRemainingOption);

        command.SetAction(async parseResult =>
        {
            var names = parseResult.GetValue(namesOption);
            var quantity = parseResult.GetValue(quantityOption);
            var isRemaining = parseResult.GetValue(isRemainingOption);

            var consumeType = isRemaining ? TransactionType.Remaining : TransactionType.Consumed;

            if (names != null)
            {
                await coffeeService.UpdateCoffeesBatchAsync(names, quantity, consumeType);

                foreach (var name in names)
                {
                    var coffee = await coffeeService.GetByNameAsync(name);

                    Console.WriteLine(coffee is not null
                        ? $"\e[32m{(isRemaining ? "Remaining" : "Consumed")} {quantity} {coffee.Brand} {coffee.Name}\e[0m"
                        : $"\e[31mCoffee not found {name}\e[0m");
                }
            }
        });

        return command;
    }
}