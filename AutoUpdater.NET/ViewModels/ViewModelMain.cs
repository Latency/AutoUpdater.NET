// ****************************************************************************
// Project:  Patch1
// File:     ViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     05/03/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET.Views;
using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelMain : DependencyObject, IViewModelMain
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static bool AllowReminder(object? _) => true;

    public ICommand CommandRemindLater { get; set; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelMain()
    {
        CommandRemindLater = new RelayCommand(((IViewModelMain)this).ButtonOk_Click, AllowReminder);
    }


    // ReSharper disable once AsyncVoidMethod
    void IViewModelMain.ButtonOk_Click(object? sender)
    {
        var win = sender as Window_Main ?? throw new NullReferenceException();
        var frm = App.GetWindow<Window_DownloadUpdate>(null);

        frm.Owner ??= win ?? throw new NullReferenceException();

        try
        {
            win.Hide();
            frm.ShowDialog();
        } finally
        {
            win.Show();
        }
    }
}