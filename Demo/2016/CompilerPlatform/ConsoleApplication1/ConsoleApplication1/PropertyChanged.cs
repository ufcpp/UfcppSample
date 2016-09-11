using System.Collections.Generic;
using System.ComponentModel;

namespace ConsoleApplication1.PropertyChanged
{
    namespace 意味のある情報
    {
        class Point
        {
            public int X;
            public int Y;
            public int Sum => X + Y;
        }
    }

    namespace 書かなきゃいけないコード
    {
        class Point : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);
            private void SetProperty<T>(ref T storage, T value, PropertyChangedEventArgs args)
            {
                if (!EqualityComparer<T>.Default.Equals(storage, value))
                {
                    storage = value;
                    OnPropertyChanged(args);
                }
            }

            public int X
            {
                get { return _x; }
                set { SetProperty(ref _x, value, XProperty); OnPropertyChanged(SumProperty); }
            }
            private int _x;
            public static readonly PropertyChangedEventArgs XProperty = new PropertyChangedEventArgs(nameof(X));

            public int Y
            {
                get { return _y; }
                set { SetProperty(ref _y, value, YProperty); OnPropertyChanged(SumProperty); }
            }
            private int _y;
            public static readonly PropertyChangedEventArgs YProperty = new PropertyChangedEventArgs(nameof(X));

            public int Sum => _x * _y;
            public static readonly PropertyChangedEventArgs SumProperty = new PropertyChangedEventArgs(nameof(Sum));
        }
    }
}
