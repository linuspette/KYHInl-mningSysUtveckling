namespace Client.Administration.MVVM.ViewModels;

internal class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        KitchenViewModel = new KitchenViewModel();
        LivingRoomViewModel = new LivingRoomViewModel();
        BedRoomViewModel = new BedRoomViewModel();

        KitchenViewCommand = new RelayCommand(x => { CurrentView = KitchenViewModel; });
        LivingRoomViewCommand = new RelayCommand(x => { CurrentView = LivingRoomViewModel; });
        BedRoomViewCommand = new RelayCommand(x => { CurrentView = BedRoomViewModel; });

        CurrentView = KitchenViewModel;
    }

    private object _currentView = null!;
    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    //Kitchen view
    public RelayCommand KitchenViewCommand { get; set; }
    public KitchenViewModel KitchenViewModel { get; set; }

    //Living room view
    public RelayCommand LivingRoomViewCommand { get; set; }
    public LivingRoomViewModel LivingRoomViewModel { get; set; }

    //Bed room view
    public RelayCommand BedRoomViewCommand { get; set; }
    public BedRoomViewModel BedRoomViewModel { get; set; }
}