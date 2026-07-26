using CoffeeInventory.Application.Services;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class AddCommand(CoffeeService coffeeService)
{
    public Command Build()
    {
        var command = new Command("Add", "Add a new coffee to inventory")
        {
            Aliases = { "a", "add" }
        };
        
        var brandArgument = new Argument<string>("brand")
        {
            Description = "Coffee brand",
        };
        
        var nameArgument = new Argument<string>("name")
        {
            Description = "Coffee name",
        };
        
        var isDecaffeinatedOption = new Option<bool>("--decaffeinated", "-d")
        {
            Description = "Is decaffeinated",
        };
        
        var quantityOption = new Option<int>("--quantity", "-q")
        {
            Description = "Initialize quantity",
            DefaultValueFactory = _ => 10,
        };
        
        command.Arguments.Add(brandArgument);
        command.Arguments.Add(nameArgument);
        command.Options.Add(quantityOption);
        
        command.SetAction(parseResult =>
        {
            var brand = parseResult.GetValue(brandArgument);
            var name = parseResult.GetValue(nameArgument);
            var isDecaffeinated = parseResult.GetValue(isDecaffeinatedOption);
            
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Brand and Name cannot be empty.");
                return;
            }
            
            var quantity = parseResult.GetValue(quantityOption);
            
            // TODO: Uncomment add method
            // coffeeService.AddAsync(brand, name, quantity, isDecaffeinated);
            Console.WriteLine($"New coffee added to the inventory, {quantity} pieces of {brand} {name}");
        });
        
        return command;
    }
}
