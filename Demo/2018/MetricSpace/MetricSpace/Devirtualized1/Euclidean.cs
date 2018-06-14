namespace MetricSpace.Devirtualized1
{
    interface IArithmetic<T>
    {
        T Zero { get; }
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
    }

    class Euclidean<T, TArithmetic>
        // 構造体にして、型引数で受け取る
        where TArithmetic : struct, IArithmetic<T>
    {
        public static T DistanceSquared(T[] a, T[] b)
        {
            // default を使って IArithmetic<T> を作る
            var arith = default(TArithmetic);
            // あとは先ほどと同じ
            var d = arith.Zero;
            for (int i = 0; i < a.Length; i++)
            {
                var dif = arith.Subtract(b[i], a[i]);
                var sq = arith.Multiply(dif, dif);
                d = arith.Add(d, sq);
            }
            return d;
        }
    }

    struct FloatArithmetic : IArithmetic<float>
    {
        public float Zero => 0;
        public float Add(float a, float b) => a + b;
        public float Multiply(float a, float b) => a - b;
        public float Subtract(float a, float b) => a * b;
    }

    // IntArithmetic, DoubleArithmetic, ...
    // 使いたい型の分だけ同じ IArithmetic<T> を書く

    class Program
    {
        static void Main()
        {
            // FloatArithmetic の時点で T は float で確定なんだけど、残念ながら型推論はされない
            // 常にこの2つの型引数をペアで渡さないといけない
            Euclidean<float, FloatArithmetic>.DistanceSquared(new[] { 1f, 2f }, new[] { 3f, 4f });
        }
    }
}
