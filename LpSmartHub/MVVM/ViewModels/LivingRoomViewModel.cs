using LpSmartHub.Helpers;
using LpSmartHub.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LpSmartHub.MVVM.ViewModels;

public class LivingRoomViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public ICommand NavigateToSettings { get; }

    public LivingRoomViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;

        DeviceItems = new ObservableCollection<DeviceItem>();
        NavigateToSettings =
            new NavigateCommand<LivingRoomViewModel>(navigationStore, () => new LivingRoomViewModel(_navigationStore));

        SetClock();
        GetDeviceItemsAsync().ConfigureAwait(false);
    }

    private ObservableCollection<DeviceItem>? _deviceItems;

    public ObservableCollection<DeviceItem> DeviceItems
    {
        get => _deviceItems ?? null!;
        set
        {
            _deviceItems = value;
            OnPropertyChanged();
        }
    }


    private string? _currentTime;
    public string CurrentTime
    {
        get { return _currentTime!; }
        set
        {
            _currentTime = value;
            OnPropertyChanged();
        }
    }

    private string? _currentDate;
    public string CurrentDate
    {
        get { return _currentDate!; }
        set
        {
            _currentDate = value;
            OnPropertyChanged();
        }
    }



    private void SetClock()
    {
        CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
        CurrentTime = DateTime.Now.ToString("HH:MM:ss");
    }

    protected override async void ClockTimer_Tick(object? sender, EventArgs e)
    {
        SetClock();
        await GetDeviceItemsAsync();
        base.ClockTimer_Tick(sender, e);
    }

    private async Task GetDeviceItemsAsync()
    {

    }
}