using CoffeeInventory.Domain.Enums;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeInventory.Wpf.ViewModels;

public partial class TransactionRow : ObservableObject
{
    public required Func<TransactionType, IEnumerable<string>> GetBrands { get; set; }
    public required Func<string, TransactionType, IEnumerable<string>> GetNames { get; set; }

    public ObservableCollection<TransactionType> TransactionTypes { get; } =
    [
        TransactionType.Consumed,
        TransactionType.Remaining,
        TransactionType.Replenishment
    ];

    [ObservableProperty]
    private TransactionType _selectedTransactionType;

    partial void OnSelectedTransactionTypeChanged(TransactionType value)
    {
        SelectedBrand = null;
        SelectedName = null;

        switch (value)
        {
            case TransactionType.Consumed:
                SelectedQuantity = 1;
                break;
            case TransactionType.Remaining:
                SelectedQuantity = 0;
                break;
            case TransactionType.Replenishment:
            case TransactionType.New:
                SelectedQuantity = 10;
                break;
        }

        OnPropertyChanged(nameof(Brands));
        OnPropertyChanged(nameof(Names));
    }

    public IEnumerable<string> Brands => GetBrands?.Invoke(SelectedTransactionType) ?? [];

    [ObservableProperty] 
    private string? _selectedBrand;

    partial void OnSelectedBrandChanged(string? value)
    {
        SelectedName = null;
        OnPropertyChanged(nameof(Names));
    }

    [ObservableProperty]
    private string? _selectedName;

    [ObservableProperty]
    private int _selectedQuantity = 1;

    public IEnumerable<string> Names =>
        SelectedBrand is null
            ? []
            : GetNames?.Invoke(SelectedBrand, SelectedTransactionType) ?? [];
}
