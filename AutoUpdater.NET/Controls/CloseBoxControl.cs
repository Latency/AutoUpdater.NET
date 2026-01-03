// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     CloseBoxControl.cs
// Author:   Latency McLaughlin
// Date:     01/02/2026
// ****************************************************************************

using System.ComponentModel;
using System.Windows;

namespace AutoUpdaterDotNET.Controls;

public class CloseBoxControl : FrameworkElement // or UserControl, etc.
{
    // 1. Register the Dependency Property
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State),                                             // Name of the property
                                                                                          typeof(bool?),                                             // Type of the property
                                                                                          typeof(CloseBoxControl),                                   // Owner class type
                                                                                          new FrameworkPropertyMetadata(true, OnControlBoxChanged)); // Property metadata (default value, property changed callback, etc.)

    public static Action<bool?>? StateChanged { get; set; }


    // 2. CLR Property Wrapper
    [Bindable(true)]
    public bool? State
    {
        get => (bool?)GetValue(StateProperty);
        set => SetValue(StateProperty, value!);
    }


    private static void OnControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //var control  = (RestrictedWindow)d;
        var oldValue = (bool?)e.OldValue;
        var newValue = (bool?)e.NewValue;

        if (oldValue == newValue)
            return;

        StateChanged?.Invoke(newValue);
    }
}