using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeInventory.Wpf.Behaviours;

public static class NumberOnlyBehaviour
{
    public static readonly DependencyProperty ModeProperty =
        DependencyProperty.RegisterAttached(
            "Mode",
            typeof(NumberOnlyBehaviourModes?),
            typeof(NumberOnlyBehaviour),
            new UIPropertyMetadata(null, OnValueChanged));

    public static NumberOnlyBehaviourModes? GetMode(DependencyObject o)
    {
        return (NumberOnlyBehaviourModes?)o.GetValue(ModeProperty);
    }

    public static void SetMode(DependencyObject o, NumberOnlyBehaviourModes? value)
    {
        o.SetValue(ModeProperty, value);
    }

    private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not Control uiElement) return;

        if (e.NewValue is NumberOnlyBehaviourModes)
        {
            uiElement.PreviewTextInput += OnTextInput;
            uiElement.PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(uiElement, OnPaste);
        }
        else
        {
            uiElement.PreviewTextInput -= OnTextInput;
            uiElement.PreviewKeyDown -= OnPreviewKeyDown;
            DataObject.RemovePastingHandler(uiElement, OnPaste);
        }
    }

    private static void OnTextInput(object sender, TextCompositionEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        var mode = GetMode(textBox) ?? NumberOnlyBehaviourModes.PositiveWholeNumber;

        switch (mode)
        {
            case NumberOnlyBehaviourModes.Decimal:
                if (e.Text.Any(c => !char.IsDigit(c)))
                {
                    e.Handled = true;
                }

                HandleSigns();
                HandleDecimalPoint();
                break;
            case NumberOnlyBehaviourModes.PositiveWholeNumber:
                if (e.Text.Any(c => !char.IsDigit(c)))
                {
                    e.Handled = true;
                }

                break;
            case NumberOnlyBehaviourModes.WholeNumber:
                if (e.Text.Any(c => !char.IsDigit(c)))
                {
                    e.Handled = true;
                }

                HandleSigns();
                break;
            case NumberOnlyBehaviourModes.PositiveDecimalNumber:
                if (e.Text.Any(c => !char.IsDigit(c)))
                {
                    e.Handled = true;
                }

                HandleDecimalPoint();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return;

        void HandleSigns()
        {
            if (string.IsNullOrEmpty(e.Text)) return;

            switch (e.Text[0])
            {
                case '-':
                {
                    var nonSelectedTest = GetNonSelectedTest(textBox);

                    if (nonSelectedTest.Length == 0)
                    {
                        e.Handled = false;
                    }
                    else if (nonSelectedTest.First() == '-')
                    {
                        var startPos = textBox.SelectionStart;
                        textBox.Text = nonSelectedTest[1..];
                        textBox.SelectionStart = startPos - 1;
                    }
                    else
                    {
                        var startPos = textBox.SelectionStart;
                        textBox.Text = "-" + nonSelectedTest;
                        textBox.SelectionStart = startPos + 1;
                    }

                    break;
                }
                case '+':
                {
                    var nonSelectedTest = GetNonSelectedTest(textBox);

                    if (nonSelectedTest.Length <= 0 || nonSelectedTest.First() != '-') return;
                    var startPos = textBox.SelectionStart;
                    textBox.Text = nonSelectedTest[1..];
                    textBox.SelectionStart = startPos - 1;
                    break;
                }
            }
        }

        void HandleDecimalPoint()
        {
            if (string.IsNullOrEmpty(e.Text) || e.Text[0] != '.') return;

            var nonSelectedTest = GetNonSelectedTest(textBox);

            if (nonSelectedTest.Contains('.'))
            {
                var startPos = textBox.SelectionStart;
                var decimalIndex = nonSelectedTest.IndexOf('.');
                var newText = nonSelectedTest.Replace(".", "");

                if (startPos > decimalIndex)
                    startPos--;

                textBox.Text = string.Concat(newText.AsSpan()[..startPos], ".", newText.AsSpan());
                textBox.SelectionStart = startPos + 1;
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }

    private static string GetNonSelectedTest(TextBox textBox)
    {
        var startText = textBox.SelectionStart == 0 ? string.Empty : textBox.Text[..textBox.SelectionStart];

        var endText = textBox.SelectionStart + textBox.SelectionLength == textBox.Text.Length
            ? string.Empty
            : textBox.Text[(textBox.SelectionStart + textBox.SelectionLength)..];

        return startText + endText;
    }

    private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space)
        {
            e.Handled = true;
        }
    }

    private static void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            var text = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;

            if (text.Any(c => !char.IsDigit(c)))
            {
                e.CancelCommand();
            }
        }
        else
        {
            e.CancelCommand();
        }
    }
}