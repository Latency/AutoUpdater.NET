// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ObservableObject, IViewModelUpdate
{
    public event Action<bool?>? ToggleControlBox;


    [RelayCommand]
    public void Skip() => ToggleControlBox?.Invoke(true);


    [RelayCommand]
    public void RemindLater() => ToggleControlBox?.Invoke(false);


    [RelayCommand]
    public void Update() => ToggleControlBox?.Invoke(null);
}