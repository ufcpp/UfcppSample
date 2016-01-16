namespace TurtleGraphics.ViewModels
{
    /// <summary>
    /// 亀に対する命令。
    /// </summary>
    public struct Command
    {
        public enum CommandType
        {
            /// <summary>
            /// 前進。
            /// </summary>
            Walk,

            /// <summary>
            /// 回転。
            /// </summary>
            Turn,

            /// <summary>
            /// 今ひかれている線の削除。
            /// </summary>
            Clear,

            /// <summary>
            /// 速さの変更。
            /// </summary>
            Speed,
        }

        /// <summary>
        /// 命令の種類。
        /// </summary>
        public CommandType Type { get; }

        /// <summary>
        /// 命令の引数。
        /// </summary>
        public double Value { get; }

        /// <summary></summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <param name="value"><see cref="Value"/></param>
        private Command(CommandType type, double value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// 前進。
        /// </summary>
        /// <param name="distance">前進する距離[ドット]。</param>
        /// <returns></returns>
        public static Command Walk(double distance) => new Command(CommandType.Walk, distance);

        /// <summary>
        /// 方向転換。
        /// </summary>
        /// <param name="angle">方向転換する角度[度]。</param>
        /// <returns></returns>
        public static Command Turn(double angle) => new Command(CommandType.Turn, angle);

        /// <summary>
        /// 速さの変更。
        /// </summary>
        /// <param name="speedDotPerSecond">変更後の速さ[ドット/秒]。</param>
        /// <returns></returns>
        public static Command Speed(double speedDotPerSecond) => new Command(CommandType.Speed, speedDotPerSecond);

        /// <summary>
        /// 今ひかれている線の削除。
        /// </summary>
        /// <returns></returns>
        public static Command Clear() => new Command(CommandType.Clear, 0);
    }
}
