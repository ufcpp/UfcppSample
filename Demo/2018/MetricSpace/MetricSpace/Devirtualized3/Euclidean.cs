using System;
using System.Runtime.CompilerServices;

namespace MetricSpace.Devirtualized3
{
    interface IMetric<T, TArray>
        where TArray : struct, IFixedArray<T>
    {
        T DistanceSquared(TArray a, TArray b);
    }

    struct EuclideanMetric<T, TArithmetic, TArray, TArrayAccessor> : IMetric<T, TArray>
        where TArithmetic : struct, IArithmetic<T>
        where TArray : struct, IFixedArray<T>
        where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
    {
        public T DistanceSquared(TArray a, TArray b)
        {
            var arith = default(TArithmetic);
            var accessor = default(TArrayAccessor);
            var d = arith.Zero;
            for (int i = 0; i < accessor.Length; i++)
            {
                var dif = arith.Subtract(accessor.At(ref b, i), accessor.At(ref a, i));
                var sq = arith.Multiply(dif, dif);
                d = arith.Add(d, sq);
            }
            return d;
        }
    }

    // Manhattan とか Chebychev とかも同様に作る

    class Program
    {
        // 近い方の点を求める
        static TArray Nearest<T, TArray, TMetric>(TArray origin, TArray a, TArray b)
            where T : IComparable<T>
            where TArray : struct, IFixedArray<T>
            where TMetric : struct, IMetric<T, TArray>
        {
            var metric = default(TMetric);

            var da = metric.DistanceSquared(origin, a);
            var db = metric.DistanceSquared(origin, b);

            return da.CompareTo(db) <= 0 ? a : b;
        }

        static void Main()
        {
            // 型引数は3つと思いきや、Euclidean がさらに4つ求めるので合計7つ
            // 常にこの7つの型引数が必要
            var n = Nearest<float, Fixed2<float>.Array, EuclideanMetric<float, FloatArithmetic, Fixed2<float>.Array, Fixed2<float>>>(
                (0, 0), (1, 2), (3, 4));

            Console.WriteLine((n.Item1, n.Item2));
        }
    }
}
