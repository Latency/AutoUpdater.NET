// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Config.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Views;

public partial class Window_Config
{
    [GeneratedRegex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
    private static partial Regex MyRegex();


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Config()
    {
        InitializeComponent();
    }


    private void Timers_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        var tvi  = e.Source as TreeViewItem;
        var root = tvTimers?.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;

        if (tvi == root)
            return;

        MessageBox.Show("Click");
    }


    private void Window_Main_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ViewModelConfig dc)
            return;

        if (DataContext is ViewModelConfig vm)
        {
            vm.UpdateIcon       += OnUpdateIcon;
            vm.UpdateVersion    += OnUpdateVersion;
            vm.UpdateValidation += OnUpdateValidation;

            OnUpdateIcon(vm.TmpIcon);
        }

        dc.OnLoaded();
    }


    private void OnUpdateIcon(BitmapImage? imagePath)
    {
        if (imagePath is not null)
            Icon = imagePath;
    }


    private void OnUpdateVersion(string? version)
    {
        if (LabelVersion is not null && !string.IsNullOrEmpty(version))
            LabelVersion.Text = version;
    }


    private void OnUpdateValidation(bool? isEnabled)
    {
        ButtonUpdate?.IsEnabled = isEnabled ?? false;
    }


    private void TbProxyUri_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e.Text != "\r")
            return;

        var regex = MyRegex();
        if (!regex.IsMatch(((TextBox)e.OriginalSource!).Text))
            MessageBox.Show("Invalid Input!", "Validation Format Error", MessageBoxButton.OK, MessageBoxImage.Error);

        OnUpdateValidation(null);
    }


    private void Window_Main_OnClosing(object? sender, CancelEventArgs e)
    {
        Hide();
        e.Cancel = true;
    }
}