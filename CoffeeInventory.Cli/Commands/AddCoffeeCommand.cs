using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Entities;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class AddCoffeeCommand(
    CoffeeService coffeeService, 
    BrandService brandService, 
    CapsuleTypeService capsuleTypeService,
    CupSizeService cupSizeService) : CommandBase
{
    public override Command Build()
    {
        var command = new Command("Add", "Add a new coffee to inventory")
        {
            Aliases = { "a", "add" }
        };

        var brandArgument = new Argument<string>("Brand")
        {
            Description = "Coffee brand",
        };

        var nameArgument = new Argument<string>("Name")
        {
            Description = "Coffee name",
        };

        var capsuleTypeArgument = new Argument<string>("CapsuleType")
        {
            Description = "Capsule Type",
        };

        var cupSizesOptions = new Option<string[]>("--cup-sizes", "-c")
        {
            Description = "Cup sizes",
            AllowMultipleArgumentsPerToken = true,
        };

        var isDecaffeinatedOption = new Option<bool>("--decaffeinated", "-d")
        {
            Description = "Is decaffeinated",
        };

        command.Arguments.Add(brandArgument);
        command.Arguments.Add(nameArgument);
        command.Arguments.Add(capsuleTypeArgument);
        command.Options.Add(cupSizesOptions);
        command.Options.Add(isDecaffeinatedOption);

        command.SetAction(async parseResult =>
        {
            var brand = parseResult.GetValue(brandArgument);
            var name = parseResult.GetValue(nameArgument);
            var capsuleType = parseResult.GetValue(capsuleTypeArgument);
            var cupSizes = parseResult.GetValue(cupSizesOptions);
            var isDecaffeinated = parseResult.GetValue(isDecaffeinatedOption);

            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(capsuleType))
            {
                throw new ArgumentException("Did not provide all required arguments. Brand, Name, Capsule Type");
            }

            var existingBrand = await brandService.GetByNameAsync(brand);

            if (existingBrand is null)
            {
                throw new ArgumentException($"Brand '{brand}' does not exist.");
            }

            var existingCapsuleType = await capsuleTypeService.GetByNameAsync(capsuleType);

            if (existingCapsuleType is null)
            {
                throw new ArgumentException($"Capsule Type '{capsuleType}' does not exist.");
            }

            var listOfCupSizes = new List<CupSize>();

            if (cupSizes is not null && cupSizes.Length != 0)
            {
                foreach (var cupSize in cupSizes)
                {
                    var existingCupSize = await cupSizeService.GetByNameAsync(cupSize);

                    if (existingCupSize is null)
                    {
                        Console.WriteLine($"Capsule Type '{capsuleType}' does not exist.");
                        return;
                    }

                    listOfCupSizes.Add(existingCupSize);
                }
            }

            var coffee = new Coffee
            {
                Brand = existingBrand,
                Name = name,
                IsDecaffeinated = isDecaffeinated,
                CapsuleType = existingCapsuleType,
                CupSizes = listOfCupSizes,
                Quantity = 0,
                Consumed = 0,
            };

            await coffeeService.AddAsync(coffee);
        });

        return command;
    }
}
