using CoffeeInventory.Application.Services;
using CoffeeInventory.Domain.Entities;
using System.Collections.ObjectModel;

namespace CoffeeInventory.Wpf.States;

public class InventoryStore
{
    private readonly CoffeeService _coffeeService;
    private readonly CapsuleTypeService _capsuleTypeService;
    private readonly CupSizeService _cupSizeService;
    private readonly BrandService _brandService;

    public ObservableCollection<Coffee> Coffees { get; } = [];
    public ObservableCollection<Coffee> Inventory { get; } = [];
    public ObservableCollection<CapsuleType> CapsuleTypes { get; } = [];
    public ObservableCollection<CupSize> CupSizes { get; } = [];
    public ObservableCollection<Brand> Brands { get; } = [];

    public InventoryStore(
        CoffeeService coffeeService, 
        CapsuleTypeService capsuleTypeService,
        CupSizeService cupSizeService,
        BrandService brandService)
    {
        _coffeeService = coffeeService;
        _capsuleTypeService = capsuleTypeService;
        _cupSizeService = cupSizeService;
        _brandService = brandService;
    }

    public async Task InitializeAsync()
    {
        await LoadCoffeesAsync();
        await LoadCapsuleTypesAsync();
        await LoadCupSizesAsync();
        await LoadBrandsAsync();
    }

    #region Coffee

    public async Task LoadCoffeesAsync()
    {
        var coffees = await _coffeeService.GetAllAsync();

        Inventory.Clear();
        Coffees.Clear();

        foreach (var coffee in coffees)
        {
            Inventory.Add(coffee);
            Coffees.Add(coffee);
        }
    }

    public async Task UpdateCoffeeAsync(Coffee coffee)
    {
        await _coffeeService.UpdateAsync(coffee);

        await LoadCoffeesAsync();
    }

    public async Task SaveAsNewCoffeeAsync(Coffee coffee)
    {
        await _coffeeService.AddAsync(coffee);

        await LoadCoffeesAsync();
    }

    public async Task DeleteCoffeeAsync(Coffee coffee)
    {
        await _coffeeService.DeleteAsync(coffee);

        await LoadCoffeesAsync();
    }

    #endregion

    #region Brand

    private async Task LoadBrandsAsync()
    {
        var brands =  await _brandService.GetAllAsync();
        
        Brands.Clear();
        
        foreach (var brand in brands)
        {
            Brands.Add(brand);
        }
    }

    public async Task UpdateBrand(Brand brand)
    {
        await _brandService.UpdateAsync(brand);

        await LoadBrandsAsync();
    }

    public async Task SaveAsNewBrand(Brand brand)
    {
        await _brandService.AddAsync(brand);

        await LoadBrandsAsync();
    }

    public async Task DeleteBrand(Brand brand)
    {
        await _brandService.DeleteAsync(brand);

        await LoadBrandsAsync();
    }

    #endregion

    #region CupSize

    public async Task UpdateCupSize(CupSize cupSize)
    {
        await _cupSizeService.UpdateAsync(cupSize);

        await LoadCupSizesAsync();
    }

    public async Task SaveAsNewCupSize(CupSize cupSize)
    {
        await _cupSizeService.AddAsync(cupSize);

        await LoadCupSizesAsync();
    }

    public async Task DeleteCupSize(CupSize cupSize)
    {
        await _cupSizeService.DeleteAsync(cupSize);

        await LoadCupSizesAsync();
    }

    private async Task LoadCupSizesAsync()
    {
        var cupSizes = await _cupSizeService.GetAllAsync();

        CupSizes.Clear();

        foreach (var cupSize in cupSizes)
        {
            CupSizes.Add(cupSize);
        }
    }

    #endregion

    #region CapsuleType

    private async Task LoadCapsuleTypesAsync()
    {
        var capsuleTypes = await _capsuleTypeService.GetAllAsync();
        
        CapsuleTypes.Clear();

        foreach (var capsuleType in capsuleTypes)
        {
            CapsuleTypes.Add(capsuleType);
        }
    }

    public async Task UpdateCapsuleType(CapsuleType capsuleType)
    {
        await _capsuleTypeService.UpdateAsync(capsuleType);

        await LoadCapsuleTypesAsync();
    }

    public async Task SaveAsNewCapsuleType(CapsuleType capsuleType)
    {
        await _capsuleTypeService.AddAsync(capsuleType);

        await LoadCapsuleTypesAsync();
    }

    public async Task DeleteCapsuleType(CapsuleType capsuleType)
    {
        await _capsuleTypeService.DeleteAsync(capsuleType);

        await LoadCapsuleTypesAsync();
    }

    #endregion
}