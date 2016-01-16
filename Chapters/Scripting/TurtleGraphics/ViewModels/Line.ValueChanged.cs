using System.ComponentModel;

namespace TurtleGraphics.ViewModels
{
    partial class Line
    {
        private NotifyRecord _value;

        /// <summary>
        /// 始点のX座標。
        /// </summary>
        public double X1 { get { return _value.X1; } set { SetProperty(ref _value.X1, value, X1Property); } }
        private static readonly PropertyChangedEventArgs X1Property = new PropertyChangedEventArgs(nameof(X1));

        /// <summary>
        /// 始点のY座標。
        /// </summary>
        public double Y1 { get { return _value.Y1; } set { SetProperty(ref _value.Y1, value, Y1Property); } }
        private static readonly PropertyChangedEventArgs Y1Property = new PropertyChangedEventArgs(nameof(Y1));

        /// <summary>
        /// 終点のX座標。
        /// </summary>
        public double X2 { get { return _value.X2; } set { SetProperty(ref _value.X2, value, X2Property); } }
        private static readonly PropertyChangedEventArgs X2Property = new PropertyChangedEventArgs(nameof(X2));

        /// <summary>
        /// 終点のY座標。
        /// </summary>
        public double Y2 { get { return _value.Y2; } set { SetProperty(ref _value.Y2, value, Y2Property); } }
        private static readonly PropertyChangedEventArgs Y2Property = new PropertyChangedEventArgs(nameof(Y2));
    }
}
