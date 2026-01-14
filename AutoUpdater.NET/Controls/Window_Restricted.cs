// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Restricted.cs
// Author:   Latency McLaughlin
// Date:     01/07/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Dependency_Properties;
using System.ComponentModel;
using System.Windows;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Controls;

/// <summary>
///     Interaction logic for Window_Restricted.xaml
/// </summary>
public abstract class Window_Restricted : Window
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly OwnerConverter      _ownerConverter;
    private readonly TopmostConverter    _topmostConverter;
    private readonly ControlBoxConverter _controlBoxConverter;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Constructor
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    /// <summary>
    ///     Default Constructor
    /// </summary>
    protected Window_Restricted()
    {
        _ownerConverter       = new OwnerConverter(this);
        _topmostConverter     = new TopmostConverter(this);
        _controlBoxConverter  = new ControlBoxConverter(this);

        ResizeMode            = ResizeMode.NoResize;
        ShowActivated         = true;
        ShowInTaskbar         = true;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle           = WindowStyle.SingleBorderWindow;

        Closing += OnClosing;
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructor


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public new bool Topmost
    {
        get => _topmostConverter.Topmost;
        set => _topmostConverter.Topmost = value;
    }

    public bool? ControlBox
    {
        get => _controlBoxConverter.ControlBox;
        set => _controlBoxConverter.ControlBox = value;
    }

    [DefaultValue(null)]
    public new Window? Owner
    {
        get => _ownerConverter.Owner;
        set => _ownerConverter.Owner = value;
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    /// SetBindings
    /// </summary>
    public void SetBindings(IViewModelConfig vmc)
    {
        _ownerConverter.SetBindings(vmc);
        _topmostConverter.SetBindings(vmc);
        _controlBoxConverter.SetBindings(vmc);
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}