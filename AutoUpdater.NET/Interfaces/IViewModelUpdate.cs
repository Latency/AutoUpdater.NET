// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows.Input;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelUpdate
{
    ICommand CommandButtonSkip        { get; set; }
    ICommand CommandButtonRemindLater { get; set; }
    ICommand CommandButtonUpdate      { get; set; }

    void ButtonSkip_Click(object?        sender);
    void ButtonRemindLater_Click(object? sender);
    void ButtonUpdate_Click(object?      sender);
}