using System.Text;
using CoffeeInventory.Application.Services;
using CoffeeInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeInventory.Infrastructure;

public class DatabaseBackupRecoveryService : IDatabaseBackupRecoveryService
{
    private readonly IDbContextFactory<CoffeeInventoryDbContext> _dbContextFactory;

    public DatabaseBackupRecoveryService(IDbContextFactory<CoffeeInventoryDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task BackUpToCsvAsync(string path)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var coffees = await dbContext.Coffees.ToListAsync();
        
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Id,Brand,Name,Quantity,Consumed,IsDecaffeinated");
        
        foreach (var coffee in coffees)
        {
            stringBuilder.AppendLine(
                $"{coffee.Id},{coffee.Brand},{coffee.Name},{coffee.Quantity},{coffee.Consumed},{coffee.IsDecaffeinated}");
        }
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        
        await using var streamWriter = new StreamWriter(Path.Combine(path, $"{DateTime.Today:yyyy-MM-dd}.csv"));
        await streamWriter.WriteAsync(stringBuilder.ToString());
    }
    
    public async Task RecoveryFromCsvAsync(string path)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        using var streamReader = new StreamReader(path);
        
        var line = await streamReader.ReadLineAsync();
        
        while (line is not null)
        {
            if (line.StartsWith("Id"))
            {
                line = await streamReader.ReadLineAsync();
                continue;
            }
            
            var values = line.Split(',');
            
            if (values.Length != 7)
            {
                throw new ArgumentException("Invalid CSV format.");
            }
            
            var coffee = dbContext.Coffees.FirstOrDefault(c => c.Id == Guid.Parse(values[0]));
            
            if (coffee is null)
            {
                throw new ArgumentNullException(nameof(coffee), "Coffee does not exist.");
            }
            
            coffee.Quantity = int.Parse(values[4]);
            coffee.Consumed = int.Parse(values[5]);
            coffee.IsDecaffeinated = bool.Parse(values[6]);
            
            dbContext.Coffees.Update(coffee);
            
            line = await streamReader.ReadLineAsync();
        }
        
        await dbContext.SaveChangesAsync();
    }
}
