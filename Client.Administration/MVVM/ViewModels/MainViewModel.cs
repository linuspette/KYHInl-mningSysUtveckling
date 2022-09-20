namespace Client.Administration.MVVM.ViewModels;

internal class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        KitchenVM = new KitchenViewModel();
        CurrentView = KitchenVM;

        KitchenViewCommand = new RelayCommand(x => { CurrentView = KitchenVM; });
    }

    private object _currentView = null!;

    public RelayCommand KitchenViewCommand { get; set; } = null!;
    public KitchenViewModel KitchenVM { get; set; } = null!;

    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
}