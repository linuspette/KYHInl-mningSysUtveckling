using LpSmartHub.MVVM.ViewModels;
using System;

namespace LpSmartHub.Helpers;

public class NavigationStore
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