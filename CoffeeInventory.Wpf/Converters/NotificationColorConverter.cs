using CoffeeInventory.Application.Services;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CoffeeInventory.Wpf.Converters;

internal class NotificationColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not NotificationType type)
        {
            return Brushes.Black;
        }

        return type switch
        {
            NotificationType.None => Brushes.White,
            NotificationType.Information => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
            NotificationType.Success => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")),
            NotificationType.Warning => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
            NotificationType.Error => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")),
            _ => Brushes.Black,
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}