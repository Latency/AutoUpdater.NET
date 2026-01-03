// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelRemindLater : ObservableObject
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [ObservableProperty]
    public partial RemindLaterFormat RemindLaterFormat { get; private set; }

    [ObservableProperty]
    public partial int RemindLaterAt { get; private set; }

    [ObservableProperty]
    public partial bool OptionYesSelected { get; set; }

    [ObservableProperty]
    public partial bool OptionNoSelected { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}