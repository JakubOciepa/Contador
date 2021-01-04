using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contador.Mobile.ViewModels
{
    /// <summary>
    /// View model base class. Implements <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        ///<inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        ///<inheritdoc/>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Updates value of the 'field' argument with value given by the second argument.
        /// Notifies the application about update of the property which has executed this method.
        /// </summary>
        /// <param name="field">Property's backing field.</param>
        /// <param name="value">New value to set.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">Property type.</typeparam>
        /// <returns>True if property value changed, false otherwise.</returns>
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
