using System.ComponentModel;

namespace ValueChanged
{
    partial class Sample
    {
        private NotifyRecord _value;

        public int X { get { return _value.X; } set { SetProperty(ref _value.X, value, XProperty); } }
        private static readonly PropertyChangedEventArgs XProperty = new PropertyChangedEventArgs(nameof(X));
        public int Y { get { return _value.Y; } set { SetProperty(ref _value.Y, value, YProperty); } }
        private static readonly PropertyChangedEventArgs YProperty = new PropertyChangedEventArgs(nameof(Y));
    }
}
