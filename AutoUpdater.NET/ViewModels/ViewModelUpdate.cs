// ****************************************************************************
// Project:  Patch1
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     05/03/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelUpdate : DependencyObject, IViewModelUpdate
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static bool AllowSkip       (object? _) => true;
    private static bool AllowRemindLater(object? _) => true;
    private static bool AllowUpdate     (object? _) => true;

    public ICommand CommandButtonSkip   { get; set; }
    public ICommand CommandButtonRemindLater { get; set; }
    public ICommand CommandButtonUpdate { get; set; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelUpdate()
    {
        CommandButtonSkip        = new RelayCommand(((IViewModelUpdate)this).ButtonSkip_Click,   AllowSkip);
        CommandButtonRemindLater = new RelayCommand(((IViewModelUpdate)this).ButtonRemindLater_Click, AllowRemindLater);
        CommandButtonUpdate      = new RelayCommand(((IViewModelUpdate)this).ButtonUpdate_Click, AllowUpdate);
    }


    void IViewModelUpdate.ButtonSkip_Click(object? sender)
    {
        var win = sender as Window_Update;
        win.ToggleControlBox(true);
    }


    void IViewModelUpdate.ButtonRemindLater_Click(object? sender)
    {
        var win = sender as Window_Update;
        win.ToggleControlBox(false);
    }


    void IViewModelUpdate.ButtonUpdate_Click(object? sender)
    {
        var win = sender as Window_Update;
        win.ToggleControlBox(null);
    }
}