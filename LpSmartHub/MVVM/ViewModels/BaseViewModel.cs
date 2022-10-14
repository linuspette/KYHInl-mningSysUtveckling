using LpSmartHub.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LpSmartHub.MVVM.ViewModels;

public class BaseViewModel : Timers, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}