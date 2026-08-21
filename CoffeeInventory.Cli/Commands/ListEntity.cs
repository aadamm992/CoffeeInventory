using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Entities;
using System.CommandLine;

namespace CoffeeInventory.Cli.Commands;

internal class ListEntity(
    CoffeeService coffeeService,
    BrandService brandService,
    CapsuleTypeService capsuleTypeService,
    CupSizeService cupSizeService) : CommandBase
{
    public override Command Build()
    {
        var command = new Command("List", "List of availivable entity")
        {
            Aliases = { "l", "list" }
        };

        var coffeeOptions = new Option<bool>("--list-coffees");
        var brandOptions = new Option<bool>("--list-brands");
        var capsuleTypeOptions = new Option<bool>("--list-capsule-types");
        var cupSizeOptions = new Option<bool>("--list-cup-sizes");

        command.Options.Add(coffeeOptions);
        command.Options.Add(brandOptions);
        command.Options.Add(capsuleTypeOptions);
        command.Options.Add(cupSizeOptions);

        command.SetAction(async parseResult =>
        {
            var isListOfCoffees = parseResult.GetValue(coffeeOptions);
            var isListOfBrands = parseResult.GetValue(brandOptions);
            var isListOfCapsuleTypes = parseResult.GetValue(capsuleTypeOptions);
            var isListOfCupSizes = parseResult.GetValue(cupSizeOptions);

            if (isListOfCoffees)
            {
                await ListOfEntity(coffeeService);
            }

            if (isListOfBrands)
            {
                await ListOfEntity(brandService);
            }

            if (isListOfCapsuleTypes)
            {
                await ListOfEntity(capsuleTypeService);
            }

            if (isListOfCupSizes)
            {
                await ListOfEntity(cupSizeService);
            }
        });

        return command;
    }

    private static async Task ListOfEntity<TEntity>(ServiceBase<TEntity> service)
        where TEntity : class
    {
        var entities = await service.GetAllAsync();

        Console.WriteLine($"> {typeof(TEntity).Name}");

        foreach(var entity in entities)
        {
            Console.WriteLine($"- {entity}");
        }

        Console.WriteLine();
    }
}
