// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_RemindLater.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Dependency_Properties;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Views;

public partial class Window_RemindLater : Window_Restricted
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly ComboBoxSelectedItemConverter _comboBoxSelectedItemConverter;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    public Window_RemindLater()
    {
        InitializeComponent();

        _comboBoxSelectedItemConverter = new(this, ComboBoxRemindLater);
    }


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings(IViewModelConfig vmc)
    {
        base.SetBindings(vmc);
        _comboBoxSelectedItemConverter.SetBindings(vmc);
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}