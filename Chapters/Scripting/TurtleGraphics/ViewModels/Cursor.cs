namespace TurtleGraphics.ViewModels
{
    /// <summary>
    /// 亀の現在位置を示すカーソル。
    /// </summary>
    public partial class Cursor : BindableBase
    {
        struct NotifyRecord
        {
            /// <summary>
            /// X 座標。
            /// </summary>
            public double X;

            /// <summary>
            /// Y 座標。
            /// </summary>
            public double Y;

            /// <summary>
            /// 向き。
            /// 単位は度(degree)。
            /// 真上を0度として、時計回りが正。
            /// </summary>
            public double Direction;
        }
    }
}
