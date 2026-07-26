namespace CoffeeInventory.Application.Services;

public class NotificationService()
{
    public event Action<string, NotificationType>? OnNotify;

    public void Notify(string message, NotificationType type)
    {
        OnNotify?.Invoke(message, type);
    }
}
