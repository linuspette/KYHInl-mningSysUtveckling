using System;
using System.Windows.Input;

namespace Client.Administration.MVVM;

internal class RelayCommand : ICommand
{
    private Action<object> _execute = null!;
    private Func<object, bool> _canExecute = null!;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null!)
    {
        _execute = execute;
        _canExecute = canExecute;

    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter!);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter!);
    }
}