using System.Windows;
using System.Windows.Input;

namespace CoffeeInventory.Wpf;

public partial class MainWindow : Window
{
    public MainWindow(object dataContext)
    {
        InitializeComponent();

        DataContext = dataContext;
    }
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control) return;

        int? targetIndex = e.Key switch
        {
            Key.D1 or Key.NumPad1 => 0,
            Key.D2 or Key.NumPad2 => 1,
            Key.D3 or Key.NumPad3 => 2,
            Key.D4 or Key.NumPad4 => 3,
            Key.D5 or Key.NumPad5 => 4,
            Key.D6 or Key.NumPad6 => 5,
            Key.D7 or Key.NumPad7 => 6,
            Key.D8 or Key.NumPad8 => 7,
            Key.D9 or Key.NumPad9 => 8,
            _ => null
        };

        if (targetIndex is { } index && index < MainTabControl.Items.Count)
        {
            MainTabControl.SelectedIndex = index;
            e.Handled = true;
        }
    }
}