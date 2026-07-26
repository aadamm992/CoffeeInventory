using CoffeeInventory.Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Threading;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class NotificationViewModel : ObservableObject, IDisposable
{
    private readonly NotificationService _notificationService;
    private readonly DispatcherTimer _hideTimer;
    private TimeSpan _autoHideDuration;

    public NotificationViewModel(NotificationService notificationService)
    {
        _notificationService = notificationService;
        _notificationService.OnNotify += HandleNotification;

        _hideTimer = new DispatcherTimer();
        _hideTimer.Tick += (_, _) => ClearNotification();
        _autoHideDuration = TimeSpan.FromSeconds(10);
        _hideTimer.Interval = _autoHideDuration;
    }

    [ObservableProperty]
    private string? _message;

    [ObservableProperty]
    private NotificationType _type;

    public void Show(string message, NotificationType type)
    {
        Show(message, type, null);
    }

    public void Show(string message, NotificationType type, TimeSpan? duration)
    {
        Message = message;
        Type = type;
        StartHideTimer(duration);
    }

    private void StartHideTimer(TimeSpan? duration)
    {
        _hideTimer.Stop();
        _hideTimer.Interval = duration ?? _autoHideDuration;
        _hideTimer.Start();
    }

    private void ClearNotification()
    {
        _hideTimer.Stop();
        Message = null;
        Type = default;
    }

    private void HandleNotification(string message, NotificationType type)
    {
        var formatted = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
        Show(formatted, type);
    }

    public void Dispose()
    {
        _hideTimer.Stop();
        _notificationService.OnNotify -= HandleNotification;
    }
}