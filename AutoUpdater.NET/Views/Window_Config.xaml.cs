// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Config.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.ViewModels;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.Extensions;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace AutoUpdaterDotNET.Views;

public partial class Window_Config
{
    [GeneratedRegex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
    private static partial Regex MyRegex();

    private Config?           _config;
    private ContentControl?   _cc;


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

        _config = (Config) vm.Config;

        _config.UpdateIcon       += OnUpdateIcon;
        _config.UpdateVersion    += OnUpdateVersion;
        _config.UpdateValidation += OnUpdateValidation;
        _config.UpdateTitle      += OnUpdateTitle;
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

        var regex = MyRegex();
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
        if (_config?.FtpProfile is null)
            return;

        var cb = (ComboBox)sender;
        _config.FtpProfile.Encoding = cb.SelectedItem switch
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
        FtpEncodingPropertyGrid!.SelectedObject = _config.FtpProfile.Encoding;
    }


    private void CbHasCredentials_OnChecked(object sender, RoutedEventArgs e) => FtpNetworkCredentials?.Visibility = Visibility.Visible;


    private void CbHasCredentials_OnUnchecked(object sender, RoutedEventArgs e) => FtpNetworkCredentials?.Visibility = Visibility.Collapsed;


    private void CbHasCredentials_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox { IsChecked: true })
            CbHasCredentials_OnChecked(sender, e);
        else
            CbHasCredentials_OnUnchecked(sender, e);
    }


    private void WatermarkPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        switch (sender)
        {
            case WatermarkPasswordBox box:
                _config?.FtpProfile?.Credentials?.Password       = box.Password;
                _config?.FtpProfile?.Credentials?.SecurePassword = box.SecurePassword;
                break;
            case WatermarkTextBox box:
                _config?.FtpProfile?.Credentials?.Password = box.Text;
                break;
        }
    }


    private void FtpNetworkCredentials_OnExpanded(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);
            Dispatcher?.Invoke(() =>
            {
                var cb = FtpCredentialsPropertyGrid?.FindVisualChild<CheckBox>();
                if (cb is not null)
                    SecurePassword_OnClick(cb, new RoutedEventArgs());
            });
        });
    }


    private void SecurePassword_OnClick(object sender, RoutedEventArgs e)
    {
        var cb     = sender as CheckBox;
        var passwd = _config?.FtpProfile?.Credentials?.Password!;
        _cc?.ContentTemplate = (TryFindResource(cb?.IsChecked == true ? "SecurePasswordBox" : "UnsecuredPasswordBox") as DataTemplate)!;

        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);
            Dispatcher?.Invoke(() =>
            {
                if (cb?.IsChecked == true)
                    FtpCredentialsPropertyGrid?.FindVisualChild<WatermarkPasswordBox>("SecurePasswordBox")?.Password = passwd;
                else
                    FtpCredentialsPropertyGrid?.FindVisualChild<WatermarkTextBox>("UnsecuredPasswordBox")?.Text = passwd;
            });
        });
    }


    private void CbWindowSizeOverride_OnChecked(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);

            Dispatcher?.Invoke(() =>
            {
                var size = _config?.WindowSize;
                if (!size.HasValue)
                    return;

                WindowSizePropertyGrid?.FindVisualChild<IntegerUpDown>("WindowSizeHeight")?.Value = (int)size.Value.Height;
                WindowSizePropertyGrid?.FindVisualChild<IntegerUpDown>("WindowSizeWidth")?.Value  = (int)size.Value.Width;
            });
        });
    }


    private void PasswordTextBox_OnLoaded(object sender, RoutedEventArgs e) => _cc = sender as ContentControl;
}