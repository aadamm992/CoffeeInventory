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

public partial class CapsuleTypeControlViewModel : ObservableObject
{
    private readonly ILogger<CapsuleTypeControlViewModel> _logger;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;

    public CapsuleTypeControlViewModel(ILogger<CapsuleTypeControlViewModel> logger, InventoryStore inventoryStore, NotificationService notificationService)
    {
        _logger = logger;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;

        CapsuleTypesView = CollectionViewSource.GetDefaultView(_inventoryStore.CapsuleTypes);
    }

    public ICollectionView CapsuleTypesView { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private CapsuleType? _selectedCapsuleType;

    partial void OnSelectedCapsuleTypeChanging(CapsuleType? value)
    {
        if (value == null) return;

        Name = value.Name;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    private string? _name;

    private bool CanExecute()
    {
        return SelectedCapsuleType is not null || !(string.IsNullOrEmpty(Name));
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Update()
    {
        if (SelectedCapsuleType == null || string.IsNullOrEmpty(Name)) return;

        try
        {
            SelectedCapsuleType.Name = Name;

            await _inventoryStore.UpdateCapsuleType(SelectedCapsuleType);
            CapsuleTypesView.Refresh();
            //_logger.LogInformation("Update Capsule Type: {CapsuleType}", SelectedCapsuleType.Name);
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

        var capsuleType = new CapsuleType
        {
            Name = Name,
        };

        try
        {
            await _inventoryStore.SaveAsNewCapsuleType(capsuleType);
            CapsuleTypesView.Refresh();
            //_logger.LogInformation("Save as new Capsule Type: {CapsuleType}", capsuleType.Name);
            _notificationService.Notify("Save as new Capsule Type successfully.", NotificationType.Success);
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
        if (SelectedCapsuleType == null) return;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this record? This action cannot be undone.",
            "Delete",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        if (messageBoxResult != MessageBoxResult.Yes) return;

        try
        {
            await _inventoryStore.DeleteCapsuleType(SelectedCapsuleType);
            CapsuleTypesView.Refresh();
            //_logger.LogInformation("Deleted Capsule Type: {CapsuleType}", SelectedCapsuleType.Name);
            SelectedCapsuleType = null;
            _notificationService.Notify("Capsule Type deletion successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
