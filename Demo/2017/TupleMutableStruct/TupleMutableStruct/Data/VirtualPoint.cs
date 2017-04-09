namespace TupleMutableStruct.Data.Virtual
{
    using System.ComponentModel;

    // クラスの場合は virtual にすることでプロパティにする意味もある
    public class Point
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
    }

    // 例えば、既存の型に対して変更トラッキング機能を追加したい場面がある
    internal class PointImpl : Point, INotifyPropertyChanging, INotifyPropertyChanged
    {
        public override int X
        {
            get => base.X;
            set
            {
                if (base.X != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(X)));
                    base.X = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
                }
            }
        }

        // X と同様なので Y は省略
        public override int Y
        {
            get => base.Y;
            set
            {
                if (base.Y != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Y)));
                    base.Y = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
