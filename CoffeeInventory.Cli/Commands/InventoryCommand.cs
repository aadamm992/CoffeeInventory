using System.CommandLine;
using Microsoft.IdentityModel.Tokens;
using CoffeeInventory.Application.Services;

namespace CoffeeInventory.Cli.Commands;

internal class InventoryCommand(CoffeeService coffeeService)
{
    public Command Build()
    {
        var command = new Command("Inventory", "Shows the coffees in inventory")
        {
            Aliases = { "i", "inventory" },
        };

        var fullInventory = new Option<bool>("--show-all", "-s")
        {
            Description = "Show all coffees",
            DefaultValueFactory = _ => false,
        };

        command.Options.Add(fullInventory);

        command.SetAction(async parseResult =>
        {
            var showAll = parseResult.GetValue(fullInventory);
            var coffees = (await coffeeService.GetAllAsync())
                .Where(c => showAll || c.Quantity > 0)
                .ToList();

            if (coffees.IsNullOrEmpty())
            {
                Console.WriteLine("Inventory is empty. No more coffee.");
                return;
            }

            var longestBrandCount = coffees.Select(c => c.Brand.Name).Max(c => c.Length);
            var longestNameCount = coffees.Select(c => c.Name).Max(c => c.Length);
            var longestQuantity = coffees.Max(c => c.Quantity).ToString().Length;
            var longestConsumed = coffees.Max(c => c.Consumed).ToString().Length;

            foreach (var coffee in coffees)
            {
                var brandSeparator = RepeatChar('-', longestBrandCount - coffee.Brand.Name.Length + 1);
                var nameSeparator = RepeatChar('-', longestNameCount - coffee.Name.Length + 1 + longestQuantity - coffee.Quantity.ToString().Length);
                var quantitySeparator = RepeatChar('-', longestConsumed - coffee.Consumed.ToString().Length + 1);

                Console.WriteLine($"{coffee.Brand} {brandSeparator} {coffee.Name} {nameSeparator} {coffee.Quantity} {quantitySeparator} {coffee.Consumed}");
            }
        });

        return command;
    }

    private static string RepeatChar(char c, int n)
    {
        return string.Concat(Enumerable.Repeat(c, n));
    }
}
