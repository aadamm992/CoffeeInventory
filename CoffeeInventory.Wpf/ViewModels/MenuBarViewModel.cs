using CoffeeInventory.Application.Services;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class MenuBarViewModel : ObservableObject
{
    private readonly ILogger<MenuBarViewModel> _logger;
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;
    private readonly IDatabaseBackupRecoveryService _databaseBackupRecoveryService;
    
    public MenuBarViewModel(
        ILogger<MenuBarViewModel> logger,
        InventoryStore inventoryStore, 
        NotificationService notificationService, 
        IDatabaseBackupRecoveryService databaseBackupRecoveryService)
    {
        _logger = logger;
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;
        _databaseBackupRecoveryService = databaseBackupRecoveryService;
    }

    [RelayCommand]
    private async Task DatabaseBackupToCsvButton()
    {
        await _databaseBackupRecoveryService.BackupAsync();
        _notificationService.Notify("Database backup completed successfully.", NotificationType.Success);
    }

    [RelayCommand]
    private static void ExitButton() 
    {
        System.Windows.Application.Current.Shutdown(); 
    }
}