namespace CoffeeInventory.Wpf.ViewModels;

public class MainViewModel
{
    public MenuBarViewModel MenuBarViewModel { get; }
    public NotificationViewModel NotificationViewModel { get; }
    public TransactionViewModel TransactionViewModel { get; }
    public InventoryViewModel InventoryViewModel { get; }
    public CoffeeControlViewModel CoffeeControlViewModel { get; }
    public ControlsViewModel ControlsViewModel { get; }
    
    public MainViewModel(
        MenuBarViewModel menuBarViewModel,
        NotificationViewModel notificationViewModel,
        TransactionViewModel transactionViewModel,
        InventoryViewModel inventoryViewModel,
        CoffeeControlViewModel coffeeControlViewModel,
        ControlsViewModel controlsViewModel)
    {
        MenuBarViewModel = menuBarViewModel;
        NotificationViewModel = notificationViewModel;
        TransactionViewModel = transactionViewModel;
        InventoryViewModel = inventoryViewModel;
        CoffeeControlViewModel = coffeeControlViewModel;
        ControlsViewModel = controlsViewModel;
    }
}
