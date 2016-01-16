namespace TurtleGraphics.ViewModels
{
    /// <summary>
    /// 亀が通った道に引く線。
    /// </summary>
    public partial class Line : BindableBase
    {
        struct NotifyRecord
        {
            /// <summary>
            /// 始点のX座標。
            /// </summary>
            public double X1;

            /// <summary>
            /// 始点のY座標。
            /// </summary>
            public double Y1;

            /// <summary>
            /// 終点のX座標。
            /// </summary>
            public double X2;

            /// <summary>
            /// 終点のY座標。
            /// </summary>
            public double Y2;
        }
    }
}
