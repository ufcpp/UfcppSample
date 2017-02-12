namespace Deconstruction._Vector2D
{
    using static System.Math;

    struct Radian
    {
        public double Value { get; }
        public Radian(double value) => Value = value;
    }

    struct Vector2D
    {
        public double X { get; }
        public double Y { get; }

        // コンストラクターは当然、個数が同じでも、型が違えば呼び分けができる
        public Vector2D(double x, double y) => (X, Y) = (x, y);
        public Vector2D(double radius, Radian angle)
            : this(radius * Cos(angle.Value), radius * Sin(angle.Value)) { }

        // 引数の数が同じ Deconstruct が2つある
        // 片方だけならいいけど、2つあると分解ができなくなる
        public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
#if false
        public void Deconstruct(out double radius, out Radian angle)
            => (radius, angle) = (Sqrt(X * X + Y * Y), new Radian(Atan2(Y, X)));
#endif
    }

    class Program
    {
        static void Main()
        {
            // コンストラクターの呼び分け
            var p = new Vector2D(1, 2);
            var q = new Vector2D(10, new Radian(PI / 5));

            // 分解は呼び分けできない
            (double x, double y) = q; // コンパイル エラー
#if false
            (double r, Radian a) = p; // コンパイル エラー
#endif
        }
    }
}
