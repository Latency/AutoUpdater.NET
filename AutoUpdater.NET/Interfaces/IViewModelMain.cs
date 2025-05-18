// ****************************************************************************
// Project:  BHI
// File:     IViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************

using System.Windows.Input;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMain
{
    void ButtonOk_Click(object? sender);

    ICommand CommandRemindLater { get; set; }
}