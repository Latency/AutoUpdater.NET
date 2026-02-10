// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Config.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.ViewModels;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Views;

public partial class Window_Config : Window_Restricted
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
        if (DataContext is not ViewModelConfig vm)
            return;

        vm.UpdateIcon       += OnUpdateIcon;
        vm.UpdateVersion    += OnUpdateVersion;
        vm.UpdateValidation += OnUpdateValidation;
        vm.UpdateTitle      += OnUpdateTitle;

        vm.TmpIcon = FindResource("project") as BitmapImage;

        OnUpdateIcon(vm.TmpIcon);
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


    private void OnUpdateTitle(string? title)
    {
        if (string.IsNullOrEmpty(title))
            title = string.Concat(Assembly.GetEntryAssembly()!.FullName!.TakeWhile(c => c != ','));

        if (Owner?.Title == title)
            return;

        Owner?.Title = title;
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


    private void TbAppTitle_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataContext is not ViewModelConfig vm)
            return;

        var text = (e.Source as TextBox)?.Text;
        if (string.IsNullOrEmpty(text))
            return;

        vm.AppTitle = text;
    }
}