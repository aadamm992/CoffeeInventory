using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Entities;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class BrandControlViewModel : ObservableObject
{
    private readonly ILogger<BrandControlViewModel> _logger;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;

    public BrandControlViewModel(ILogger<BrandControlViewModel> logger, InventoryStore inventoryStore, NotificationService notificationService)
    {
        _logger = logger;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;

        CupSizesView = CollectionViewSource.GetDefaultView(_inventoryStore.Brands);
    }

    public ICollectionView CupSizesView { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private Brand? _selectedBrand;

    partial void OnSelectedBrandChanging(Brand? value)
    {
        if (value == null) return;

        Name = value.Name;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    private string? _name;

    private bool CanExecute()
    {
        return SelectedBrand is not null || !(string.IsNullOrEmpty(Name));
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Update()
    {
        if (SelectedBrand == null || string.IsNullOrEmpty(Name)) return;

        try
        {
            SelectedBrand.Name = Name;

            await _inventoryStore.UpdateBrand(SelectedBrand);
            CupSizesView.Refresh();
            //_logger.LogInformation("Update Brand: {Brand}", SelectedBrand.Name);
            _notificationService.Notify("Update successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task SaveAsNew()
    {
        if (string.IsNullOrEmpty(Name)) return;

        var brand = new Brand
        {
            Name = Name,
        };

        try
        {
            await _inventoryStore.SaveAsNewBrand(brand);
            CupSizesView.Refresh();
            //_logger.LogInformation("Save as new Brand: {Brand}", brand.Name);
            _notificationService.Notify("Add new Brand successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Delete()
    {
        if (SelectedBrand == null) return;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this record? This action cannot be undone.",
            "Delete",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        if (messageBoxResult != MessageBoxResult.Yes) return;

        try
        {
            await _inventoryStore.DeleteBrand(SelectedBrand);
            CupSizesView.Refresh();
            //_logger.LogInformation("Deleted Brand: {Brand}", SelectedBrand.Name);
            SelectedBrand = null;
            _notificationService.Notify("Brand deletion successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
