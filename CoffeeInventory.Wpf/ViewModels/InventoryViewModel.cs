using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    private readonly InventoryStore _inventoryStore;
    
    public InventoryViewModel(InventoryStore inventoryStore)
    {
        _inventoryStore = inventoryStore;
        
        InventoryView = CollectionViewSource.GetDefaultView(_inventoryStore.Inventory);
        
        InventoryView.Filter = obj =>
        {
            if (obj is not Coffee coffee) return false;
            return !IsAvailableCoffees || coffee.Quantity > 0;
        };
        
        _inventoryStore.Inventory.CollectionChanged += OnStoreCollectionChanged;
        
        foreach (var item in _inventoryStore.Inventory)
        {
            AttachItemPropertyChanged(item);
        }
        
        IsAvailableCoffees = true;
        
        RefreshQuantities();
    }
    
    public ICollectionView InventoryView { get; }
    
    [ObservableProperty]
    private bool _isAvailableCoffees;
    
    partial void OnIsAvailableCoffeesChanged(bool value)
    {
        InventoryView.Refresh();
    }
    
    [ObservableProperty]
    private int _availableCoffeesQuantity;
    
    [ObservableProperty]
    private int _consumedCoffeesQuantity;
    
    private void OnStoreCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var old in e.OldItems.OfType<Coffee>())
            {
                DetachItemPropertyChanged(old);
            }
        }
        
        if (e.NewItems != null)
        {
            foreach (var @new in e.NewItems.OfType<Coffee>())
            {
                AttachItemPropertyChanged(@new);
            }
        }
        
        InventoryView.Refresh();
        RefreshQuantities();
    }
    
    private void AttachItemPropertyChanged(Coffee coffee)
    {
        if (coffee is INotifyPropertyChanged notifyPropertyChanged)
        {
            notifyPropertyChanged.PropertyChanged += OnCoffeePropertyChanged;
        }
    }
    
    private void DetachItemPropertyChanged(Coffee coffee)
    {
        if (coffee is INotifyPropertyChanged npc)
        {
            npc.PropertyChanged -= OnCoffeePropertyChanged;
        }
    }
    
    private void RefreshQuantities()
    {
        AvailableCoffeesQuantity = _inventoryStore.Inventory
            .Where(c => c.Quantity > 0)
            .Sum(c => c.Quantity);
        
        ConsumedCoffeesQuantity = _inventoryStore.Inventory.Sum(c => c.Consumed);
        OnPropertyChanged(nameof(ConsumedCoffeesQuantity));
    }
    
    private void OnCoffeePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not ("Quantity" or "Consumed")) return;
        InventoryView.Refresh();
        RefreshQuantities();
    }
}
