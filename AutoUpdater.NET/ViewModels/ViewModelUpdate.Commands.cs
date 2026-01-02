// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate
{
    public event Action<bool?>? ToggleControlBox;


    [RelayCommand]
    public void Skip() => ToggleControlBox?.Invoke(true);


    [RelayCommand]
    public void RemindLater() => ToggleControlBox?.Invoke(false);


    [RelayCommand]
    public void Update() => ToggleControlBox?.Invoke(null);
}