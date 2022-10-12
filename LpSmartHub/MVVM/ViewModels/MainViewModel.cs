using LpSmartHub.Helpers;

namespace LpSmartHub.MVVM.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrenViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}