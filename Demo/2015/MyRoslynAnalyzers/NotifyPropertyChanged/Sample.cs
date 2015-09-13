using System.ComponentModel;

namespace NotifyPropertyChanged
{
    [Notify]
    class Sample : INotifyPropertyChanged
    {
        public int X { get { return x; } set { SetProperty(ref x, value, xPropertyChangedEventArgs); } }
        public int Y { get { return y; } set { SetProperty(ref y, value, yPropertyChangedEventArgs); } }

        #region NotifyPropertyChangedGenerator

        public event PropertyChangedEventHandler PropertyChanged;

        private int x;
        private static readonly PropertyChangedEventArgs xPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(X));
        private int y;
        private static readonly PropertyChangedEventArgs yPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Y));

        private void SetProperty<T>(ref T field, T value, PropertyChangedEventArgs ev)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, ev);
            }
        }

        #endregion
    }
}
