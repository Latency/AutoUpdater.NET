// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.Map.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace AutoUpdaterDotNET.Models;

public partial class WindowService
{
    public void AddChild(object value) => ((IAddChild)_window).AddChild(value);

    public void AddText(string text) => ((IAddChild)_window).AddText(text);

    public void AddHandler(RoutedEvent routedEvent, Delegate handler) => _window.AddHandler(routedEvent, handler);

    public bool CaptureMouse() => _window.CaptureMouse();

    public bool CaptureStylus() => _window.CaptureStylus();

    public bool Focus() => _window.Focus();

    public void RaiseEvent(RoutedEventArgs e) => _window.RaiseEvent(e);

    public void ReleaseMouseCapture() => _window.ReleaseMouseCapture();

    public void ReleaseStylusCapture() => _window.ReleaseStylusCapture();

    public void RemoveHandler(RoutedEvent routedEvent, Delegate handler) => _window.RemoveHandler(routedEvent, handler);

    public bool Focusable
    {
        get => _window.Focusable;
        set => _window.Focusable = value;
    }

    public bool IsEnabled => _window.IsEnabled;

    public bool IsKeyboardFocused => _window.IsKeyboardFocused;

    public bool IsKeyboardFocusWithin => _window.IsKeyboardFocusWithin;

    public bool IsMouseCaptured => _window.IsMouseCaptured;

    public bool IsMouseDirectlyOver => _window.IsMouseDirectlyOver;

    public bool IsMouseOver => _window.IsMouseOver;

    public bool IsStylusCaptured => _window.IsStylusCaptured;

    public bool IsStylusDirectlyOver => _window.IsStylusDirectlyOver;

    public bool IsStylusOver => _window.IsStylusOver;

    public event KeyboardFocusChangedEventHandler? GotKeyboardFocus
    {
        add => _window.GotKeyboardFocus += value;
        remove => _window.GotKeyboardFocus -= value;
    }

    public event MouseEventHandler? GotMouseCapture
    {
        add => _window.GotMouseCapture += value;
        remove => _window.GotMouseCapture -= value;
    }

    public event StylusEventHandler? GotStylusCapture
    {
        add => _window.GotStylusCapture += value;
        remove => _window.GotStylusCapture -= value;
    }

    public event KeyEventHandler? KeyDown
    {
        add => _window.KeyDown += value;
        remove => _window.KeyDown -= value;
    }

    public event KeyEventHandler? KeyUp
    {
        add => _window.KeyUp += value;
        remove => _window.KeyUp -= value;
    }

    public event KeyboardFocusChangedEventHandler? LostKeyboardFocus
    {
        add => _window.LostKeyboardFocus += value;
        remove => _window.LostKeyboardFocus -= value;
    }

    public event MouseEventHandler? LostMouseCapture
    {
        add => _window.LostMouseCapture += value;
        remove => _window.LostMouseCapture -= value;
    }

    public event StylusEventHandler? LostStylusCapture
    {
        add => _window.LostStylusCapture += value;
        remove => _window.LostStylusCapture -= value;
    }

    public event MouseEventHandler? MouseEnter
    {
        add => _window.MouseEnter += value;
        remove => _window.MouseEnter -= value;
    }

    public event MouseEventHandler? MouseLeave
    {
        add => _window.MouseLeave += value;
        remove => _window.MouseLeave -= value;
    }

    public event MouseButtonEventHandler? MouseLeftButtonDown
    {
        add => _window.MouseLeftButtonDown += value;
        remove => _window.MouseLeftButtonDown -= value;
    }

    public event MouseButtonEventHandler? MouseLeftButtonUp
    {
        add => _window.MouseLeftButtonUp += value;
        remove => _window.MouseLeftButtonUp -= value;
    }

    public event MouseEventHandler? MouseMove
    {
        add => _window.MouseMove += value;
        remove => _window.MouseMove -= value;
    }

    public event MouseButtonEventHandler? MouseRightButtonDown
    {
        add => _window.MouseRightButtonDown += value;
        remove => _window.MouseRightButtonDown -= value;
    }

    public event MouseButtonEventHandler? MouseRightButtonUp
    {
        add => _window.MouseRightButtonUp += value;
        remove => _window.MouseRightButtonUp -= value;
    }

    public event MouseWheelEventHandler? MouseWheel
    {
        add => _window.MouseWheel += value;
        remove => _window.MouseWheel -= value;
    }

    public event KeyboardFocusChangedEventHandler? PreviewGotKeyboardFocus
    {
        add => _window.PreviewGotKeyboardFocus += value;
        remove => _window.PreviewGotKeyboardFocus -= value;
    }

    public event KeyEventHandler? PreviewKeyDown
    {
        add => _window.PreviewKeyDown += value;
        remove => _window.PreviewKeyDown -= value;
    }

    public event KeyEventHandler? PreviewKeyUp
    {
        add => _window.PreviewKeyUp += value;
        remove => _window.PreviewKeyUp -= value;
    }

    public event KeyboardFocusChangedEventHandler? PreviewLostKeyboardFocus
    {
        add => _window.PreviewLostKeyboardFocus += value;
        remove => _window.PreviewLostKeyboardFocus -= value;
    }

    public event MouseButtonEventHandler? PreviewMouseLeftButtonDown
    {
        add => _window.PreviewMouseLeftButtonDown += value;
        remove => _window.PreviewMouseLeftButtonDown -= value;
    }

    public event MouseButtonEventHandler? PreviewMouseLeftButtonUp
    {
        add => _window.PreviewMouseLeftButtonUp += value;
        remove => _window.PreviewMouseLeftButtonUp -= value;
    }

    public event MouseEventHandler? PreviewMouseMove
    {
        add => _window.PreviewMouseMove += value;
        remove => _window.PreviewMouseMove -= value;
    }

    public event MouseButtonEventHandler? PreviewMouseRightButtonDown
    {
        add => _window.PreviewMouseRightButtonDown += value;
        remove => _window.PreviewMouseRightButtonDown -= value;
    }

    public event MouseButtonEventHandler? PreviewMouseRightButtonUp
    {
        add => _window.PreviewMouseRightButtonUp += value;
        remove => _window.PreviewMouseRightButtonUp -= value;
    }

    public event MouseWheelEventHandler? PreviewMouseWheel
    {
        add => _window.PreviewMouseWheel += value;
        remove => _window.PreviewMouseWheel -= value;
    }

    public event StylusButtonEventHandler? PreviewStylusButtonDown
    {
        add => _window.PreviewStylusButtonDown += value;
        remove => _window.PreviewStylusButtonDown -= value;
    }

    public event StylusButtonEventHandler? PreviewStylusButtonUp
    {
        add => _window.PreviewStylusButtonUp += value;
        remove => _window.PreviewStylusButtonUp -= value;
    }

    public event StylusDownEventHandler? PreviewStylusDown
    {
        add => _window.PreviewStylusDown += value;
        remove => _window.PreviewStylusDown -= value;
    }

    public event StylusEventHandler? PreviewStylusInAirMove
    {
        add => _window.PreviewStylusInAirMove += value;
        remove => _window.PreviewStylusInAirMove -= value;
    }

    public event StylusEventHandler? PreviewStylusInRange
    {
        add => _window.PreviewStylusInRange += value;
        remove => _window.PreviewStylusInRange -= value;
    }

    public event StylusEventHandler? PreviewStylusMove
    {
        add => _window.PreviewStylusMove += value;
        remove => _window.PreviewStylusMove -= value;
    }

    public event StylusEventHandler? PreviewStylusOutOfRange
    {
        add => _window.PreviewStylusOutOfRange += value;
        remove => _window.PreviewStylusOutOfRange -= value;
    }

    public event StylusSystemGestureEventHandler? PreviewStylusSystemGesture
    {
        add => _window.PreviewStylusSystemGesture += value;
        remove => _window.PreviewStylusSystemGesture -= value;
    }

    public event StylusEventHandler? PreviewStylusUp
    {
        add => _window.PreviewStylusUp += value;
        remove => _window.PreviewStylusUp -= value;
    }

    public event TextCompositionEventHandler? PreviewTextInput
    {
        add => _window.PreviewTextInput += value;
        remove => _window.PreviewTextInput -= value;
    }

    public event StylusButtonEventHandler? StylusButtonDown
    {
        add => _window.StylusButtonDown += value;
        remove => _window.StylusButtonDown -= value;
    }

    public event StylusButtonEventHandler? StylusButtonUp
    {
        add => _window.StylusButtonUp += value;
        remove => _window.StylusButtonUp -= value;
    }

    public event StylusDownEventHandler? StylusDown
    {
        add => _window.StylusDown += value;
        remove => _window.StylusDown -= value;
    }

    public event StylusEventHandler? StylusEnter
    {
        add => _window.StylusEnter += value;
        remove => _window.StylusEnter -= value;
    }

    public event StylusEventHandler? StylusInAirMove
    {
        add => _window.StylusInAirMove += value;
        remove => _window.StylusInAirMove -= value;
    }

    public event StylusEventHandler? StylusInRange
    {
        add => _window.StylusInRange += value;
        remove => _window.StylusInRange -= value;
    }

    public event StylusEventHandler? StylusLeave
    {
        add => _window.StylusLeave += value;
        remove => _window.StylusLeave -= value;
    }

    public event StylusEventHandler? StylusMove
    {
        add => _window.StylusMove += value;
        remove => _window.StylusMove -= value;
    }

    public event StylusEventHandler? StylusOutOfRange
    {
        add => _window.StylusOutOfRange += value;
        remove => _window.StylusOutOfRange -= value;
    }

    public event StylusSystemGestureEventHandler? StylusSystemGesture
    {
        add => _window.StylusSystemGesture += value;
        remove => _window.StylusSystemGesture -= value;
    }

    public event StylusEventHandler? StylusUp
    {
        add => _window.StylusUp += value;
        remove => _window.StylusUp -= value;
    }

    public event TextCompositionEventHandler? TextInput
    {
        add => _window.TextInput += value;
        remove => _window.TextInput -= value;
    }

    public string? Name
    {
        get => _window.Name;
        set => _window.Name = value!;
    }

    public void BeginInit() => _window.BeginInit();

    public void EndInit() => _window.EndInit();

    public bool IsAmbientPropertyAvailable(string propertyName) => ((IQueryAmbient)_window).IsAmbientPropertyAvailable(propertyName);

    public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock) => _window.ApplyAnimationClock(dp, clock);

    public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior) => _window.ApplyAnimationClock(dp, clock, handoffBehavior);

    public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation) => _window.BeginAnimation(dp, animation);

    public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation, HandoffBehavior handoffBehavior) => _window.BeginAnimation(dp, animation, handoffBehavior);

    public object? GetAnimationBaseValue(DependencyProperty dp) => _window.GetAnimationBaseValue(dp);

    public bool               HasAnimatedProperties => _window.HasAnimatedProperties;

    public ResourceDictionary Resources
    {
        get => _window.Resources;
        set => _window.Resources = value;
    }

    public string? Title
    {
        get => _window.Title;
        set => _window.Title = value!;
    }

    public double Height
    {
        get => _window.Height;
        set => _window.Height = value;
    }

    public double Width
    {
        get => _window.Width;
        set => _window.Width = value;
    }

    public void Show() => _window.Show();

    public void Hide() => _window.Hide();

    public bool? ShowDialog() => _window.ShowDialog();

    bool IWindowService.UserResized => false;
}