// ****************************************************************************
// Project:  Patch1
// File:     RelayCommand.cs
// Author:   Latency McLaughlin
// Date:     05/03/2024
// ****************************************************************************

using System.Windows.Input;

namespace AutoUpdaterDotNET.Commands;

public class RelayCommand(Action<object?> execute, Predicate<object?>? canExecute) : ICommand
{
    private readonly Action<object?>? _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    public RelayCommand(Action<object?> execute) : this(execute, null)
    { }

    public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object? parameter) => _execute?.Invoke(parameter);
}