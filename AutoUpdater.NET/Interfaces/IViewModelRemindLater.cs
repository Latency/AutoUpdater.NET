// ****************************************************************************
// Project:  BHI
// File:     IViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************

using System.Windows.Input;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelRemindLater
{
    ICommand CommandButtonOk { get; set; }

    void ButtonOk_Click(object? sender);
}