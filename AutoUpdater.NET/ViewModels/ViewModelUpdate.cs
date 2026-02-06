// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ViewModelRestricted, IViewModelUpdate
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(BaseServiceDependencies dependencies) : base(dependencies)
    {
    }
}