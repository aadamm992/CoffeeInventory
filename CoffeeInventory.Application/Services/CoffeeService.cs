using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Enums;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CoffeeService
{
    private readonly ICoffeeRepository _coffeeRepository;

    public CoffeeService(ICoffeeRepository coffeeRepository)
    {
        _coffeeRepository = coffeeRepository;
    }

    public async Task<Coffee?> GetByNameAsync(string coffeeName)
    {
        return await _coffeeRepository.GetByNameAsync(coffeeName);
    }

    public async Task<IReadOnlyList<Coffee>> GetAllAsync()
    {
        return await _coffeeRepository.GetAllAsync();
    }

    public async Task UpdateAsync(Coffee coffee)
    {
        await _coffeeRepository.UpdateAsync(coffee);
    }

    public async Task UpdateAsync(string name, int quantity, TransactionType transactionType)
    {
        var coffee = await _coffeeRepository.GetByNameAsync(name);

        if (coffee == null) return;

        QuantityResolver(coffee, quantity, transactionType);

        await _coffeeRepository.UpdateAsync(coffee);
    }

    public async Task UpdateCoffeesBatchAsync(IEnumerable<string> names, int quantity, TransactionType transactionType)
    {
        var coffees = await _coffeeRepository.GetByNamesAsync(names);

        foreach (var coffee in coffees)
        {
            QuantityResolver(coffee, quantity, transactionType);
            await _coffeeRepository.UpdateAsync(coffee);
        }
    }

    public async Task AddAsync(Coffee coffee)
    {
        await _coffeeRepository.AddAsync(coffee);
    }

    public async Task AddAsync(
        Guid brandId,
        string name,
        int quantity,
        bool isDecaffeinated = false,
        Guid? capsuleTypeId = null,
        IEnumerable<Guid>? cupSizeIds = null)
    {
        var coffee = new Coffee
        {
            Name = name,
            Quantity = quantity,
            IsDecaffeinated = isDecaffeinated,
        };

        await _coffeeRepository.AddAsync(brandId, coffee, capsuleTypeId, cupSizeIds);
    }

    public async Task DeleteAsync(Coffee coffee)
    {
        await _coffeeRepository.DeleteAsync(coffee);
    }

    private static void QuantityResolver(Coffee coffee, int quantity, TransactionType transactionType)
    {
        switch (transactionType)
        {
            case TransactionType.Consumed:
                if (coffee.Quantity < quantity)
                {
                    throw new InvalidOperationException($"The amount of coffee to be consumed is greater than the amount in stock. To be consumed: {quantity}, Available: {coffee.Quantity}");
                }

                coffee.Quantity -= quantity;
                coffee.Consumed += quantity;
                break;
            case TransactionType.Remaining:
                var consumed = coffee.Quantity - quantity;
                if (consumed < 0)
                {
                    throw new InvalidOperationException("You cannot consume more coffee than is in stock.");
                }

                coffee.Quantity = quantity;
                coffee.Consumed += consumed;
                break;
            case TransactionType.Replenishment: coffee.Quantity += quantity; break;
            case TransactionType.New:
            default:
                throw new ArgumentOutOfRangeException(nameof(transactionType), $"Invalid transaction type. {transactionType}");
        }
    }
}
