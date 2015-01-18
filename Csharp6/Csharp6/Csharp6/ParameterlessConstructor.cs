namespace Csharp6.Csharp6.ParameterlessConstructor
{
    struct Point
    {
        public int X;
        public int Y;

        // C# 5.0 以前ではコンパイル エラーになる
        public Point()
        {
            X = int.MinValue;
            Y = int.MinValue;
        }
    }
}
