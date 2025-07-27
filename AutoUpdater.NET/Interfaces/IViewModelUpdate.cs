// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelUpdate
{
    event Action<bool?>? ToggleControlBox;

    void Skip();
    void RemindLater();
    void Update();
}