// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelDownloadUpdate : ViewModelRestricted
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelDownloadUpdate(ViewModelConfig vmMain) : base(vmMain)
    {
    }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    [ObservableProperty]
    public partial double ProgressPercentage { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}