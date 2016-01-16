using System.ComponentModel;

namespace TurtleGraphics.ViewModels
{
    partial class Cursor
    {
        private NotifyRecord _value;

        /// <summary>
        /// X 座標。
        /// </summary>
        public double X { get { return _value.X; } set { SetProperty(ref _value.X, value, XProperty); } }
        private static readonly PropertyChangedEventArgs XProperty = new PropertyChangedEventArgs(nameof(X));

        /// <summary>
        /// Y 座標。
        /// </summary>
        public double Y { get { return _value.Y; } set { SetProperty(ref _value.Y, value, YProperty); } }
        private static readonly PropertyChangedEventArgs YProperty = new PropertyChangedEventArgs(nameof(Y));

        /// <summary>
        /// 向き。
        /// 単位は度(degree)。
        /// 真上を0度として、時計回りが正。
        /// </summary>
        public double Direction { get { return _value.Direction; } set { SetProperty(ref _value.Direction, value, DirectionProperty); } }
        private static readonly PropertyChangedEventArgs DirectionProperty = new PropertyChangedEventArgs(nameof(Direction));
    }
}
