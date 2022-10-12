using Client.Administration.Annotations;
using Client.Administration.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Administration.MVVM.ViewModels;

internal class BaseViewModel : Timers, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
