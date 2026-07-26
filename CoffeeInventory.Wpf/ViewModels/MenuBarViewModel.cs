using System.Diagnostics;
using System.IO;
using System.Windows;
using CoffeeInventory.Application.Services;
using CoffeeInventory.Wpf.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class MenuBarViewModel : ObservableObject
{
    private readonly InventoryStore _inventoryStore;
    private readonly NotificationService _notificationService;
    private readonly IDatabaseBackupRecoveryService _databaseBackupRecoveryService;
    
    private readonly string _backupFolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CoffeeInventory",
        "backup");

    public MenuBarViewModel(InventoryStore inventoryStore, NotificationService notificationService, IDatabaseBackupRecoveryService databaseBackupRecoveryService)
    {
        _inventoryStore = inventoryStore;
        _notificationService = notificationService;
        _databaseBackupRecoveryService = databaseBackupRecoveryService;
    }

    [RelayCommand]
    private async Task DatabaseBackupToCsvButton()
    {
        await _databaseBackupRecoveryService.BackUpToCsvAsync(_backupFolderPath);
        _notificationService.Notify("Database backup completed successfully.", NotificationType.Success);
    }

    [RelayCommand]
    private async Task DatabaseRecoveryFromCsvButton()
    {
        var openFileDialog = new OpenFileDialog
        {
            DefaultDirectory = _backupFolderPath,
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            Title = "Select a CSV file to recover the database from"
        };

        if (openFileDialog.ShowDialog() != true) return;

        var filePath = openFileDialog.FileName;

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            _notificationService.Notify("Invalid file path selected.", NotificationType.Error);
            return;
        }

        try
        {
            await _databaseBackupRecoveryService.RecoveryFromCsvAsync(openFileDialog.FileName);
            await _inventoryStore.LoadCoffeesAsync();
            _notificationService.Notify("Database recovery completed successfully.", NotificationType.Success);
        }
        catch (Exception)
        {
            _notificationService.Notify($"Database recovery failed.", NotificationType.Error);
        }
    }

    [RelayCommand]
    public void OpenBackupDirectoryButton()
    {
        if (!Directory.Exists(_backupFolderPath))
        {
            MessageBox.Show("Backup directory does not exist. There probably aren't any backups saved yet.", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = _backupFolderPath,
            UseShellExecute = true
        });
    }

    [RelayCommand]
    public static void ExitButton() 
    {
        System.Windows.Application.Current.Shutdown(); 
    }
}