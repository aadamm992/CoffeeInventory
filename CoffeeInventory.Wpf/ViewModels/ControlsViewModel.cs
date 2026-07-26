namespace CoffeeInventory.Wpf.ViewModels;

public class ControlsViewModel
{
    public BrandControlViewModel BrandControlViewModel { get; }
    public CoffeeControlViewModel CoffeeControlViewModel { get; }
    public CapsuleTypeControlViewModel CapsuleTypeControlViewModel { get; }
    public CupSizeControlViewModel CupSizeControlViewModel { get; }
    
    public ControlsViewModel(
        BrandControlViewModel brandControlViewModel,
        CoffeeControlViewModel coffeeControlViewModel,
        CapsuleTypeControlViewModel capsuleTypeControlViewModel,
        CupSizeControlViewModel cupSizeControlViewModel)
    {
        BrandControlViewModel = brandControlViewModel;
        CoffeeControlViewModel = coffeeControlViewModel;
        CapsuleTypeControlViewModel = capsuleTypeControlViewModel;
        CupSizeControlViewModel = cupSizeControlViewModel;
    }
}
