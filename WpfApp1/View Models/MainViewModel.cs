// ****************************************************************************
// Project:  WpfApp1
// File:     MainViewModel.cs
// Author:   Latency McLaughlin
// Date:     10/14/2025
// ****************************************************************************

using AutoUpdaterDotNET.Views;
using System.ComponentModel;
using System.Windows.Input;
using AutoUpdaterDotNET.Commands;
using WpfApp1.Interfaces;

namespace WpfApp1.View_Models;

public class MainViewModel : IMainViewModel
{
    private readonly IServiceProvider _serviceProvider = null!;

    public ICommand CloseButtonCommand { get; }
    public ICommand OpenButtonCommand { get; }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void                            OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    /// <summary>
    ///     Constructor
    /// </summary>
    public MainViewModel(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }


    /// <summary>
    ///     Constructor
    /// </summary>
    public MainViewModel()
    {
        CloseButtonCommand = new RelayCommand(ExecuteCloseButtonCommand, CanExecuteCloseButtonCommand);
        OpenButtonCommand  = new RelayCommand(ExecuteOpenButtonCommand,  CanExecuteOpenButtonCommand);
    }


    private void ExecuteOpenButtonCommand(object? parameter)
    {
        // Logic to be executed when the button is clicked
        // You can access the 'parameter' if you've set CommandParameter in XAML
        var frm = _serviceProvider.GetService(typeof(Window_Main)) as Window_Main;
        frm?.ShowDialog();
    }


    private void ExecuteCloseButtonCommand(object? parameter)
    {
        // Logic to be executed when the button is clicked
        // You can access the 'parameter' if you've set CommandParameter in XAML
        var frm = _serviceProvider.GetService(typeof(Window_Main)) as Window_Main;
        frm?.Close();
    }


    private static bool CanExecuteCloseButtonCommand(object? parameter)
    {
        // Logic to determine if the button should be enabled/disabled
        // Return true to enable, false to disable
        return true; // Always enabled in this example
    }


    private static bool CanExecuteOpenButtonCommand(object? parameter)
    {
        // Logic to determine if the button should be enabled/disabled
        // Return true to enable, false to disable
        return true; // Always enabled in this example
    }
}