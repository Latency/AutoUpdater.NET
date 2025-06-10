// ****************************************************************************
// Project:  Patch1
// File:     Window_Main.xaml.cs
// Author:   Latency McLaughlin
// Date:     04/16/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Interfaces;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_Main
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Main(IViewModelMain vm)
    {
        InitializeComponent();

        DataContext = vm;

        if (!string.IsNullOrEmpty(ConfigPath?.Text))
            ConfigPath.Text = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName + @"\Properties\AutoUpdate.json";

        if (!string.IsNullOrEmpty(LabelVersion?.Text))
        {
            var format = LabelVersion.Text;
            var ver    = Assembly.GetEntryAssembly()!.GetName().Version!;
            LabelVersion.Text = string.Format(format!, $"{ver.Major}.{ver.Minor}.{ver.Build}");
        }

        vm.CollectionChanged += (sender, _) => imgIcon!.Source = (sender as BitmapImage)!;
        vm.ImageUri          =  Icon?.ToString() ?? throw new InvalidOperationException();
    }


    private void Timers_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        var tvi = e.Source as TreeViewItem;
        var root = tvTimers?.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;

        if (tvi == root)
            return;

        MessageBox.Show("Click");
    }


    /// <summary>
    ///     Validation for URI textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TbProxyUri_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e.Text != "\r")
            return;

        var regex = MyRegex();
        if (!regex.IsMatch(((TextBox)e.OriginalSource!).Text))
            MessageBox.Show("Invalid Input!", "Validation Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }


    [GeneratedRegex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
    private static partial Regex MyRegex();
}