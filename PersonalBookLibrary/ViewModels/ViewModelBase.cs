using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalBookLibrary.ViewModels;

public abstract class ViewModelBase
    : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
        {
            return false;
        }

        field = newValue;
        RaisePropertyChanged(propertyName);
        
        return true;
    }

    protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
