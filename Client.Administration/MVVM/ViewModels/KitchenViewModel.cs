using Client.Administration.Helpers;
using System;
using System.Windows.Input;

namespace Client.Administration.MVVM.ViewModels;

internal class KitchenViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;


    public ICommand NavigateToSettings { get; }

    public KitchenViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;

        NavigateToSettings =
            new NavigateCommand<KitchenViewModel>(navigationStore, () => new KitchenViewModel(_navigationStore));

        SetClock();
    }

    private string? _currentTemperature;

    public string CurrentTemperature
    {
        get => _currentTemperature!;
        set
        {
            _currentTemperature = value;
            OnPropertyChanged();
        }
    }

    private string? _currentTime;

    public string CurrentTime
    {
        get => _currentTime!;
        set
        {
            _currentTime = value;
            OnPropertyChanged();
        }
    }
    private string? _currentDate;

    public string CurrentDate
    {
        get => _currentDate!;
        set
        {
            _currentDate = value;
            OnPropertyChanged();
        }
    }


    private void SetClock()
    {
        CurrentTime = DateTime.Now.ToString("HH:MM:ss");
        CurrentDate = DateTime.Now.ToString("dd MMMM yyyy");
    }

    protected override void ClockTimer_Tick(object? sender, EventArgs e)
    {
        SetClock();
        base.ClockTimer_Tick(sender, e);
    }
}