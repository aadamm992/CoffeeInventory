using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Enums;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class TransactionViewModel : ObservableObject
{
    private readonly ILogger<TransactionViewModel> _logger;
    private readonly CoffeeService _coffeeService;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;

    public TransactionViewModel(
        ILogger<TransactionViewModel> logger,
        CoffeeService coffeeService, 
        InventoryStore inventoryStore, 
        NotificationService notificationService)
    {
        _logger = logger;
        _coffeeService = coffeeService;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;
    }

    public ObservableCollection<TransactionRow> TransactionRows { get; } = [];
    
    [ObservableProperty]
    private TransactionRow? _selectedTransactionRow;

    [RelayCommand]
    private void AddTransactionRowButton()
    {
        TransactionRows.Add(new TransactionRow
        {
            GetBrands = GetBrands,
            GetNames = GetNames
        });

        SelectedTransactionRow = TransactionRows.First();
    }
    
    [RelayCommand]
    private void RemoveTransactionRowButton()
    {
        if (SelectedTransactionRow == null) return;

        TransactionRows.Remove(SelectedTransactionRow);

        if (TransactionRows.Count > 0)
        {
            SelectedTransactionRow = TransactionRows.First();
        }
    }

    [RelayCommand]
    private void ClearAllTransactionRowButton()
    {
        SelectedTransactionRow = null;
        TransactionRows.Clear();
    }
    
    [RelayCommand]
    private async Task SubmitButton()
    {
        try
        {
            if (TransactionRows.Count == 0) return;
            
            foreach (var row in TransactionRows)
            {
                if (string.IsNullOrWhiteSpace(row.SelectedBrand))
                {
                    _notificationService.Notify($"Brand is not selected in row {TransactionRows.IndexOf(row) + 1}", NotificationType.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(row.SelectedName))
                {
                    _notificationService.Notify($"Name is not selected in row {TransactionRows.IndexOf(row) + 1}", NotificationType.Warning);
                    return;
                }
                
                await _coffeeService.UpdateAsync(row.SelectedName, row.SelectedQuantity, row.SelectedTransactionType);
            }
            
            _notificationService.Notify("All transactions is successful", NotificationType.Success);
            
            await _inventoryStore.LoadCoffeesAsync();
            
            TransactionRows.Clear();
        }
        catch (Exception)
        {
            _notificationService.Notify("Transaction failed.", NotificationType.Error);
        }
    }
    
    private IEnumerable<string> GetBrands(TransactionType transactionType)
    {
        return _inventoryStore.Inventory
            .Where(c => transactionType != TransactionType.Consumed &&
                transactionType != TransactionType.Remaining || c.Quantity > 0)
            .Select(c => c.Brand.Name)
            .Distinct();
    }
    
    private IEnumerable<string> GetNames(string brand, TransactionType transactionType)
    {
        return _inventoryStore.Inventory
            .Where(c => c.Brand.Name == brand)
            .Where(c => transactionType != TransactionType.Consumed &&
                transactionType != TransactionType.Remaining || c.Quantity > 0)
            .Select(c => c.Name)
            .Distinct();
    }
}
