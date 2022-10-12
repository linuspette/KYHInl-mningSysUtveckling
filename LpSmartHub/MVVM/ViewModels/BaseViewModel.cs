using LpSmartHub.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LpSmartHub.MVVM.ViewModels;

public class BaseViewModel : Timers, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}