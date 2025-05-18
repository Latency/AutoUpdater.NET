// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Update.xaml.cs
// Author:   Latency McLaughlin
// Date:     05/17/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Controls;

namespace AutoUpdaterDotNET.Views;

/// <summary>
///     Interaction logic for RemindLater.xaml
/// </summary>
public sealed partial class Window_Update : RestrictedWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="vm"></param>
    public Window_Update(IViewModelUpdate vm)
    {
        InitializeComponent();

        DataContext = vm;

        Title = "{0} is available!";

        //ButtonSkip.Visibility        = AutoUpdater.ShowSkipButton ? Visibility.Visible : Visibility.Hidden;
        //ButtonRemindLater.Visibility = AutoUpdater.ShowRemindLaterButton ? Visibility.Visible : Visibility.Hidden;
        //var resources = new ComponentResourceManager(typeof(UpdateForm));
        //Text = string.Format(resources.GetString("$this.Text", CultureInfo.CurrentCulture)!,
        //                     AutoUpdater.AppTitle, _args.CurrentVersion);
        //labelUpdate.Text = string.Format(resources.GetString("labelUpdate.Text", CultureInfo.CurrentCulture)!,
        //                                 AutoUpdater.AppTitle);

        //if (LabelDescription != null && !string.IsNullOrEmpty(LabelDescription.Text))
        //    LabelDescription.Text = string.Format(LabelDescription.Text, _args.CurrentVersion, _args.InstalledVersion);

        //if (AutoUpdater.Mandatory && AutoUpdater.UpdateMode == Mode.Forced)
        //{
        //    ControlBox = false;
        //}

        //if (ButtonOk != null)
        //    ButtonOk.Click += (sender, e) => vm.CommandButtonOk.Execute(new Tuple<Window_Update, Button?, RoutedEventArgs>(this, sender as Button, e));
    }
}