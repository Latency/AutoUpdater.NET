// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Main.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Interfaces;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_Main
{
    [GeneratedRegex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
    private static partial Regex MyRegex();

    private bool _isLoaded;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Main(IViewModelMain vm)
    {
        InitializeComponent();

        DataContext        =  vm;
        vm.PropertyChanged += (_, _) => UpdateValidation();

        if (!string.IsNullOrEmpty(LabelVersion?.Text))
        {
            var format = LabelVersion.Text;
            var ver    = Assembly.GetEntryAssembly()!.GetName().Version!;
            LabelVersion.Text = string.Format(format!, $"{ver.Major}.{ver.Minor}.{ver.Build}");
        }

        if (Icon is not null)
            vm.TmpIcon = Icon;
        else
            vm.TmpIcon = Application.Current!.Resources["project"] as BitmapImage;
    }


    private void Timers_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        var tvi  = e.Source as TreeViewItem;
        var root = tvTimers?.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;

        if (tvi == root)
            return;

        MessageBox.Show("Click");
    }


    private void TbProxyUri_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e.Text != "\r")
            return;

        var regex = MyRegex();
        if (!regex.IsMatch(((TextBox)e.OriginalSource!).Text))
            MessageBox.Show("Invalid Input!", "Validation Format Error", MessageBoxButton.OK, MessageBoxImage.Error);

        UpdateValidation();
    }


    private void Window_Main_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not IViewModelMain dc)
            return;

        dc.OnLoaded(sender);

        UpdateValidation();

        _isLoaded = true;
    }


    private void UpdateValidation()
    {
        if (DataContext is not IViewModelMain dc)
            return;

        if (ButtonUpdate is not null)
            ButtonUpdate.IsEnabled = !dc.Equals();
    }


    private void CheckBox_OnClick(object sender, RoutedEventArgs e) => UpdateValidation();

    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!_isLoaded)
            return;

        UpdateValidation();
    }

    private void Iud_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (!_isLoaded)
            return;

        UpdateValidation();
    }

    private void TextBox_OnTextChanged(object  sender, TextChangedEventArgs e) => UpdateValidation();


    private void Window_Main_OnClosing(object? sender, CancelEventArgs e)
    {
        Hide();
        e.Cancel = true;
    }
}