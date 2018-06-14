namespace MetricSpace.Virtualized
{
    interface IArithmetic<T>
    {
        T Zero { get; }
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
    }

    class Euclidean<T>
    {
        // 四則演算用のインターフェイスを外からもらう
        IArithmetic<T> _arithmetic;
        public Euclidean(IArithmetic<T> arithmetic) => _arithmetic = arithmetic;


        // static にするのはあきらめる
        public T DistanceSquared(T[] a, T[] b)
        {
            var arith = _arithmetic;
            // IArithmetic<T> 越しに 0 をもらったり、四則演算したり
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
}
