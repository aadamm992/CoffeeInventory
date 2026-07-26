using CoffeeInventory.Domain.Entities;
using System.Globalization;
using System.Windows.Data;

namespace CoffeeInventory.Wpf.Converters;

internal class CupSizesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<CupSize> cupSizes)
        {
            return string.Join("\n", cupSizes.Select(cupSize => $"{cupSize.Name} ({cupSize.VolumeMl} ml)"));
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
