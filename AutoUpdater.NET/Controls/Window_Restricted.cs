// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Restricted.cs
// Author:   Latency McLaughlin
// Date:     02/12/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using WindowService.Dependency_Properties;

namespace AutoUpdaterDotNET.Controls;

/// <summary>
///     Interaction logic for Window_Restricted.xaml
/// </summary>
[Localizability(LocalizationCategory.Ignore)]
public abstract class Window_Restricted : WindowService.Controls.Window_Restricted
{
    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings<T>(T vmc)
    {
        // ReSharper disable once EntityNameCapturedOnly.Local
        // ReSharper disable once RedundantAssignment
        if (vmc is not IViewModelConfig vm)
            throw new NullReferenceException($"Unable to resolve generic type parameter '{(vmc?.GetType().Name ?? "<None>")}'");

        _ownerConverter.SetBindings(new BaseConverter.BindingObject<IConfig>
        {
            Class = vm.Config,
            Path = new PropertyPath(nameof(vm.Config.DoNotBindOwnerWindow))
        });
        _topmostConverter.SetBindings(new BaseConverter.BindingObject<IConfig>
        {
            Class = vm.Config,
            Path  = new PropertyPath(nameof(vm.Config.TopMostDisabled))
        });
        _controlBoxConverter.SetBindings(new BaseConverter.BindingObject<IConfig>
        {
            Class = vm.Config,
            Path  = new PropertyPath(nameof(vm.Config.UpdateMode))
        });
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}