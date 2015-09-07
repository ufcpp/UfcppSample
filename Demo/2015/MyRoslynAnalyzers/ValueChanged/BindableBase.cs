using System.Collections.Generic;
using System.ComponentModel;

namespace ValueChanged
{
    class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T storage, T value, PropertyChangedEventArgs args)
        {
            if(!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, args);
            }
        }
    }
}
