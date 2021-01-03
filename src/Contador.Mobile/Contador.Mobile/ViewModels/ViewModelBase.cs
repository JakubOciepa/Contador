using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contador.Mobile.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
       public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
