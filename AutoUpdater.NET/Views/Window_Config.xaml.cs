// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Config.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.DataTemplateSelectors;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.ViewModels;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid;
using MessageBox = System.Windows.MessageBox;

namespace AutoUpdaterDotNET.Views;

public partial class Window_Config
{
    [GeneratedRegex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
    private static partial Regex UriRegex();

    private          Config?                     _config;
    private          ContentControl?             _cc;
    private          PasswordBoxContentTemplate? _pbct;
    private          PropertyGrid?               _ftpPropertyGrid;
    private          PropertyItem?               _ftpEncodingPropertyItem;
    private readonly IViewModelDownloadUpdate    _vmDownloadUpdate;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Config(IViewModelDownloadUpdate vmDownloadUpdate)
    {
        _vmDownloadUpdate = vmDownloadUpdate;
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

        _config = (Config) vm.Config;

        _config.UpdateIcon       += OnUpdateIcon;
        _config.UpdateVersion    += OnUpdateVersion;
        _config.UpdateValidation += OnUpdateValidation;
        _config.UpdateTitle      += OnUpdateTitle;

        var dl                               = _vmDownloadUpdate.Download;
        tvAfterCheckForUpdates!.ItemsSource  = dl.AfterCheckForUpdatesNodeList;
        tvBeforeCheckForUpdates!.ItemsSource = dl.BeforeCheckForUpdatesNodeList;
        tvUpdateComplete!.ItemsSource        = dl.UpdateCompleteNodeList;
        tvTimers!.ItemsSource                = dl.TimerNodeList;
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


    private void OnUpdateValidation(bool? isEnabled) => ButtonUpdate?.IsEnabled = isEnabled ?? false;


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

        var regex = UriRegex();
        if (!regex.IsMatch(((TextBox)e.OriginalSource!).Text))
            MessageBox.Show("Invalid Input!", "Validation Format Error", MessageBoxButton.OK, MessageBoxImage.Error);

        OnUpdateValidation(null);
    }


    private void TbAppTitle_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var text = (e.Source as TextBox)?.Text;
        if (string.IsNullOrEmpty(text))
            return;

        if (DataContext is not ViewModelConfig vm)
            return;

        var config = (Config) vm.Config;
        config.AppTitle = text;
    }


    private void CbEncoding_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_config?.FtpProfile is null || sender is not ComboBox cb)
            return;

        var newValue = cb.SelectedItem switch
        {
            Encodings.Default          => Encoding.Default,
            Encodings.ASCII            => Encoding.ASCII,
            Encodings.BigEndianUnicode => Encoding.BigEndianUnicode,
            Encodings.Latin1           => Encoding.Latin1,
            Encodings.UTF32            => Encoding.UTF32,
            Encodings.UTF8             => Encoding.UTF8,
            Encodings.Unicode          => Encoding.Unicode,
            _                          => throw new ArgumentOutOfRangeException(nameof(cb.SelectedItem), cb.SelectedItem, null)
        };
        _config.FtpProfile.Encoding = (Encoding2) newValue;

        _ftpEncodingPropertyItem?.Value = _config.FtpProfile.Encoding;
    }


    private void WatermarkPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        switch (sender)
        {
            case WatermarkPasswordBox box:
                _config?.FtpProfile?.Credentials.Password       = box.Password ?? string.Empty;
                _config?.FtpProfile?.Credentials.SecurePassword = box.SecurePassword ?? new SecureString();
                break;
            case WatermarkTextBox box:
                _config?.FtpProfile?.Credentials.Password = box.Text;
                break;
        }
    }


    private void CbSecurePassword_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox cb)
            return;

        // Set the default content template.
        SecurePassword_OnClick(cb, new RoutedEventArgs());
    }


    private void PasswordTextBox_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not ContentControl cc)
            return;

        _cc   = cc;
        _pbct = FindResource("PasswordBoxTemplateSelector") as PasswordBoxContentTemplate;
    }


    private void SecurePassword_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not CheckBox cb || _pbct is null || _cc is null)
            return;

        _cc.ContentTemplate = cb.IsChecked is true ? _pbct.SecurePasswordTemplate : _pbct.UnsecuredPasswordTemplate;

        Dispatcher?.BeginInvoke(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250)); // Allow time for the UI to update

            if (cb.IsChecked == true)
                _ftpPropertyGrid?.FindVisualChild<WatermarkPasswordBox>("SecurePasswordBox")?.Password = _config!.FtpProfile!.Credentials.Password;
            else
                _ftpPropertyGrid?.FindVisualChild<WatermarkTextBox>("UnsecuredPasswordBox")?.Text = _config!.FtpProfile!.Credentials.Password;
        });
    }


    private void WindowSizeOverride_OnChecked(object sender, RoutedEventArgs e)
    {
        Dispatcher?.BeginInvoke(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250)); // Allow time for the UI to update

            var size = _config?.WindowSize;
            if (!size.HasValue)
                return;

            WindowSizePropertyGrid?.FindVisualChild<IntegerUpDown>("WindowSizeHeight")?.Value = (int)size.Value.Height;
            WindowSizePropertyGrid?.FindVisualChild<IntegerUpDown>("WindowSizeWidth")?.Value  = (int)size.Value.Width;
        });
    }


    private void FtpPropertyGrid_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not PropertyGrid ftpPropertyGrid)
            return;

        _ftpPropertyGrid                                        = ftpPropertyGrid;
        _ftpEncodingPropertyItem                                = ftpPropertyGrid.FindProperty("Encoding");
        ftpPropertyGrid.FindProperty("Credentials")?.IsExpanded = true;
    }
}