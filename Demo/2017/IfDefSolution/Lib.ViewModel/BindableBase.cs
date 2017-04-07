using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataLib
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        protected virtual void SetProperty<T>(ref T storage, T value, PropertyChangedEventArgs args)
        {
            if (!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                OnPropertyChanged(args);
            }
        }
        protected virtual void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) => SetProperty(ref storage, value, new PropertyChangedEventArgs(propertyName));
    }
}
