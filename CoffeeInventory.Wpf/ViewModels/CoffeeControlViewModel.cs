using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class CoffeeControlViewModel : ObservableObject
{
    private readonly ILogger<CoffeeControlViewModel> _logger;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;

    public CoffeeControlViewModel(ILogger<CoffeeControlViewModel> logger, InventoryStore inventoryStore, NotificationService notificationService)
    {
        _logger = logger;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;

        Brands = _inventoryStore.Brands;
        CapsuleTypes = _inventoryStore.CapsuleTypes;
        CoffeesView = CollectionViewSource.GetDefaultView(_inventoryStore.Coffees);
        CoffeesView.SortDescriptions.Add(new SortDescription(nameof(Name), ListSortDirection.Ascending));
    }

    public ICollectionView CoffeesView { get; }

    [ObservableProperty]
    private Coffee? _selectedCoffee;

    partial void OnSelectedCoffeeChanged(Coffee? value)
    {
        if (value is null) return;

        SelectedBrand = Brands.FirstOrDefault(brand => brand.Name == value.Brand.Name);
        Name = value.Name;
        IsDecaffeinated = value.IsDecaffeinated;
        SelectedCapsuleType = CapsuleTypes.FirstOrDefault(capsuleType => capsuleType.Name == value.CapsuleType?.Name);
        IsRistretto = value.CupSizes.Select(cupSize => cupSize.Name).Contains("Ristretto");
        IsEspresso = value.CupSizes.Select(cupSize => cupSize.Name).Contains("Espresso");
        IsLungo = value.CupSizes.Select(cupSize => cupSize.Name).Contains("Lungo");
    }

    public ObservableCollection<Brand> Brands { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private Brand? _selectedBrand;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private string? _name;

    [ObservableProperty]
    private bool _isDecaffeinated;

    public ObservableCollection<CapsuleType> CapsuleTypes { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private CapsuleType? _selectedCapsuleType;

    [ObservableProperty]
    private bool _isRistretto;

    [ObservableProperty]
    private bool _isEspresso;

    [ObservableProperty]
    private bool _isLungo;

    private bool CanExecute()
    {
        return SelectedCoffee is not null
            && SelectedBrand is not null
            && !string.IsNullOrEmpty(Name);
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Update()
    {
        if (SelectedCoffee is null || SelectedBrand is null || string.IsNullOrEmpty(Name)) return;

        try
        {
            SelectedCoffee.Brand = SelectedBrand;
            SelectedCoffee.Name = Name;
            SelectedCoffee.IsDecaffeinated = IsDecaffeinated;

            SelectedCoffee.CupSizes = new HashSet<CupSize>(GetFilteredCupSizes());

            await _inventoryStore.UpdateCoffeeAsync(SelectedCoffee);
            CoffeesView.Refresh();
            //_logger.LogInformation("Updated Coffee: {Brand}, {Name}", SelectedCoffee.Brand.Name, SelectedCoffee.Name);
            _notificationService.Notify("Coffee update successfull.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception:");
            _notificationService.Notify(ex.Message, NotificationType.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task SaveAsNew()
    {
        if (SelectedCoffee is null || SelectedBrand is null || string.IsNullOrEmpty(Name)) return;

        try
        {
            var newCoffee = new Coffee
            {
                Brand = SelectedBrand,
                Name = Name,
                CapsuleType = SelectedCapsuleType,
                IsDecaffeinated = IsDecaffeinated,
                CupSizes = new HashSet<CupSize>(GetFilteredCupSizes())
            };

            await _inventoryStore.SaveAsNewCoffeeAsync(newCoffee);
            CoffeesView.Refresh();
            //_logger.LogInformation("Save as new Coffee: {Brand}, {Name}", newCoffee.Brand.Name, newCoffee.Name);
            _notificationService.Notify($"Save as new Coffee: {newCoffee.Brand.Name}, {newCoffee.Name}", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception:");
            _notificationService.Notify(ex.Message, NotificationType.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Delete()
    {
        if (SelectedCoffee is null || SelectedBrand is null || string.IsNullOrEmpty(Name)) return;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this record? This action cannot be undone.",
            "Delete",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        if (messageBoxResult != MessageBoxResult.Yes) return;

        try
        {
            await _inventoryStore.DeleteCoffeeAsync(SelectedCoffee);
            CoffeesView.Refresh();
            //_logger.LogInformation("Deleted Coffee: {Brand}, {Name}", SelectedCoffee.Brand.Name, SelectedCoffee.Name);
            SelectedCoffee = null;
            _notificationService.Notify("Coffee deletion successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private IEnumerable<CupSize> GetFilteredCupSizes()
    {
        return _inventoryStore.CupSizes.Where(c => 
            (IsRistretto && c.Name == "Ristretto") 
            || (IsEspresso && c.Name == "Espresso") 
            || (IsLungo && c.Name == "Lungo"));
    }
}
