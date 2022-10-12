using LpSmartHub.MVVM.ViewModels;
using System;

namespace LpSmartHub.Helpers;

public class NavigateCommand<T> : BaseCommand where T : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<T> _createViewModel;

    public NavigateCommand(NavigationStore navigationStore, Func<T> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}