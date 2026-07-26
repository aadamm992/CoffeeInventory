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

public partial class CupSizeControlViewModel : ObservableObject
{
    private readonly ILogger<CupSizeControlViewModel> _logger;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;

    public CupSizeControlViewModel(ILogger<CupSizeControlViewModel> logger, InventoryStore inventoryStore, NotificationService notificationService)
    {
        _logger = logger;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;

        CupSizesView = CollectionViewSource.GetDefaultView(_inventoryStore.CupSizes);
    }

    public ICollectionView CupSizesView { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private CupSize? _selectedCupSize;

    partial void OnSelectedCupSizeChanging(CupSize? value)
    {
        if (value == null) return;

        Name = value.Name;
        VolumeMl = value.VolumeMl;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    private string? _name;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveAsNewCommand))]
    private int? _volumeMl;

    private bool CanExecute()
    {
        return SelectedCupSize is not null || !(string.IsNullOrEmpty(Name) && VolumeMl == null);
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task Update()
    {
        if (SelectedCupSize == null || string.IsNullOrEmpty(Name) || VolumeMl == null) return;

        try
        {
            SelectedCupSize.Name = Name;
            SelectedCupSize.VolumeMl = (int)VolumeMl;

            await _inventoryStore.UpdateCupSize(SelectedCupSize);
            CupSizesView.Refresh();
            //_logger.LogInformation("Update Cup Size: {CupSize}", SelectedCupSize.Name);
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
        if (string.IsNullOrEmpty(Name) || VolumeMl == null) return;

        var cupSize = new CupSize
        {
            Name = Name,
            VolumeMl = (int)VolumeMl
        };

        try
        {
            await _inventoryStore.SaveAsNewCupSize(cupSize);
            CupSizesView.Refresh();
            //_logger.LogInformation("Save as new Cup Size: {CupSize} [{Volume}]", cupSize.Name, cupSize.VolumeMl);
            _notificationService.Notify("Add new Cup Size successfully.", NotificationType.Success);
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
        if (SelectedCupSize == null) return;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this record? This action cannot be undone.",
            "Delete",
            MessageBoxButton.YesNoCancel,
            MessageBoxImage.Question,
            MessageBoxResult.Cancel);

        if (messageBoxResult != MessageBoxResult.Yes) return;

        try
        {
            await _inventoryStore.DeleteCupSize(SelectedCupSize);
            CupSizesView.Refresh();
            //_logger.LogInformation("Delete Cup Size: {CupSize}", SelectedCupSize.Name);
            SelectedCupSize = null;
            _notificationService.Notify("Cup Size deletion successfully.", NotificationType.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: ");
            _notificationService.Notify(ex.Message, NotificationType.Error);
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
