using Client.Administration.MVVM.ViewModels;
using System;

namespace Client.Administration.Helpers;

internal class NavigationStore
{
    public event Action? CurrenViewModelChanged;

    private BaseViewModel? _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel!;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrenViewModelChanged?.Invoke();
    }

}