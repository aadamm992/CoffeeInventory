using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Domain.Enums;
using CoffeeInventory.Domain.Repositories;

namespace CoffeeInventory.Application.Services;

public class CoffeeService : ServiceBase<Coffee>
{
    private readonly ICoffeeRepository _coffeeRepository;
    
    public CoffeeService(ICoffeeRepository coffeeRepository) : base(coffeeRepository)
    {
        _coffeeRepository = coffeeRepository;
    }
    
    public async Task UpdateAsync(string name, int quantity, TransactionType transactionType)
    {
        var coffee = await _coffeeRepository.GetByNameAsync(name);
        
        if (coffee == null) return;
        
        QuantityResolver(coffee, quantity, transactionType);
        
        await _coffeeRepository.UpdateAsync(coffee);
    }
    
    public async Task UpdateCoffeesBatchAsync(string[] names, int quantity, TransactionType transactionType)
    {
        var coffees = await _coffeeRepository.GetByNamesAsync(names);
        
        foreach (var coffee in coffees)
        {
            QuantityResolver(coffee, quantity, transactionType);
            await _coffeeRepository.UpdateAsync(coffee);
        }
    }
    
    public async Task AddAsync(
        Guid brandId,
        string name,
        int quantity,
        bool isDecaffeinated = false,
        Guid? capsuleTypeId = null,
        Guid[]? cupSizeIds = null)
    {
        var coffee = new Coffee
        {
            Name = name,
            Quantity = quantity,
            IsDecaffeinated = isDecaffeinated,
        };
        
        await _coffeeRepository.AddAsync(brandId, coffee, capsuleTypeId, cupSizeIds);
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
