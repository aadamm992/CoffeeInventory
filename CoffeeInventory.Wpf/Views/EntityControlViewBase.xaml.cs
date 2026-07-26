using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoffeeInventory.Wpf.Views;

public partial class EntityControlViewBase : UserControl
{
    public EntityControlViewBase()
    {
        InitializeComponent();
    }

    private static DependencyProperty EditorContentProperty =
        DependencyProperty.Register(
            nameof(EditorContent),
            typeof(object),
            typeof(EntityControlViewBase)
            );

    public object? EditorContent
    {
        get => GetValue(EditorContentProperty); 
        set => SetValue(EditorContentProperty, value);
    }

    private static readonly DependencyProperty EntityItemSourceProperty =
        DependencyProperty.Register(
            nameof(EntityItemSource),
            typeof(IEnumerable),
            typeof(EntityControlViewBase)
            );


    public IEnumerable EntityItemSource
    {
        get => (IEnumerable)GetValue(EntityItemSourceProperty);
        set => SetValue(EntityItemSourceProperty, value);
    }

    private static readonly DependencyProperty SelectedEntityProperty =
        DependencyProperty.Register(
            nameof(SelectedEntity),
            typeof(object),
            typeof(EntityControlViewBase),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

    public object? SelectedEntity
    {
        get => GetValue(SelectedEntityProperty);
        set => SetValue(SelectedEntityProperty, value);
    }

    public static readonly DependencyProperty EntityDisplayMemberPathProperty =
    DependencyProperty.Register(
        nameof(EntityDisplayMemberPath),
        typeof(string),
        typeof(EntityControlViewBase),
        new PropertyMetadata(string.Empty));

    public string EntityDisplayMemberPath
    {
        get => (string)GetValue(EntityDisplayMemberPathProperty);
        set => SetValue(EntityDisplayMemberPathProperty, value);
    }

    private static DependencyProperty UpdateCommandProperty =
        DependencyProperty.Register(
            nameof(UpdateCommand),
            typeof(ICommand),
            typeof(EntityControlViewBase)
            );

    public ICommand? UpdateCommand
    {
        get => (ICommand)GetValue(UpdateCommandProperty);
        set => SetValue(UpdateCommandProperty, value);
    }

    private static DependencyProperty SaveAsNewCommandProperty =
      DependencyProperty.Register(
          nameof(SaveAsNewCommand),
          typeof(ICommand),
          typeof(EntityControlViewBase)
          );

    public ICommand? SaveAsNewCommand
    {
        get => (ICommand)GetValue(SaveAsNewCommandProperty);
        set => SetValue(SaveAsNewCommandProperty, value);
    }

    private static DependencyProperty DeleteCommandProperty =
     DependencyProperty.Register(
         nameof(DeleteCommand),
         typeof(ICommand),
         typeof(EntityControlViewBase)
         );

    public ICommand? DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }
}
