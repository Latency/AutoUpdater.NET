// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     OwnerConverter.cs
// Author:   Latency McLaughlin
// Date:     01/13/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using System.Globalization;
using System.Windows;

namespace AutoUpdaterDotNET.Dependency_Properties;

public class OwnerConverter : BaseConverter
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly DependencyProperty _ownerProperty;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="window"></param>
    public OwnerConverter(Window window) : base(window)
    {
        _ownerProperty = DependencyProperty.Register(nameof(Owner),
                                                     typeof(Window),
                                                     window.GetType(),
                                                     new FrameworkPropertyMetadata(OnOwnerChanged)
        );
    }


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public Window? Owner
    {
        get => (Window?)GetValue(_ownerProperty);
        set => SetValue(_ownerProperty, value!);
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected override object? Converter(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var win = !(bool)value! ? (Window) parameter! : null;
        Owner = win;
        return win;
    }


    private void OnOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Parent.Owner = (e.NewValue as Window)!;
#if DEBUG
        var ownerEnabled = e.NewValue is not null;
        Parent.Title = ownerEnabled ? $"{Parent.Title}{(ownerEnabled ? $" (Owner: {e.NewValue?.GetType()})" : string.Empty)}" : Parent.Title![..Parent.Title.IndexOf('(')].TrimEnd();
#endif
    }


    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings(IViewModelConfig vm) => SetBindings(_ownerProperty, vm, nameof(vm.DoNotBindOwnerWindow), Parent.Tag);
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}