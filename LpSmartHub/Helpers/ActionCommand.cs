using System;

namespace LpSmartHub.Helpers;

public class ActionCommand : BaseCommand
{
    private readonly Action<object> _action;
    private readonly Func<object, bool> _canExecute;


    public ActionCommand(Action<object> action) : this(action, null!)
    {

    }

    public ActionCommand(Action<object> action, Func<object, bool> canExecute)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action), "Specify an Action<T>");
        _canExecute = canExecute ?? (x => true);
    }

    public override void Execute(object? parameter)
    {
#pragma warning disable CS8604
        _action(parameter);
#pragma warning restore CS8604
    }
}