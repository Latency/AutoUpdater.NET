// ****************************************************************************
// Project:  BHI
// File:     Program.cs
// Author:   Latency McLaughlin
// Date:     05/14/2025
// ****************************************************************************

using AutoUpdaterDotNET.Views;

namespace AutoUpdaterDotNET;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var application = new App();
        application.InitializeComponent();
        application.Run();
    }
}