using System;
using MetricSpace.Devirtualized3;

namespace MetricSpace.Instantiation
{
    // ジェネリックな型を1個用意しておいて、派生で型引数を与えておく
    // 数値の型
    class FloatPoint : Point<float, FloatArithmetic> { }
    class DoublePoint : Point<double, DoubleArithmetic> { }
    class IntPoint : Point<int, IntArithmetic> { }
    class ShortPoint : Point<short, ShortArithmetic> { }

    class Point<T, TArithmetic>
        where T : IComparable<T>
        where TArithmetic : struct, IArithmetic<T>
    {
        // 数値の「組」の型
        public class _1 : Dimension<Fixed1<T>.Array, Fixed1<T>> { }
        public class _2 : Dimension<Fixed2<T>.Array, Fixed2<T>> { }
        public class _3 : Dimension<Fixed3<T>.Array, Fixed3<T>> { }
        public class _4 : Dimension<Fixed4<T>.Array, Fixed4<T>> { }

        public class Dimension<TArray, TArrayAccessor>
            where TArray : struct, IFixedArray<T>
            where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
        {
            // 距離計算の方法
            public class Euclidean : Metric<EuclideanMetric<T, TArithmetic, TArray, TArrayAccessor>> { }
            public class Manhattan : Metric<ManhattanMetric<T, TArithmetic, TArray, TArrayAccessor>> { }
            public class Chebyshev : Metric<ChebyshevMetric<T, TArithmetic, TArray, TArrayAccessor>> { }

            public class Metric<TMetric>
                where TMetric : struct, IMetric<T, TArray>
            {
                public static TArray Nearest(TArray origin, TArray a, TArray b)
                {
                    var metric = default(TMetric);

                    var da = metric.DistanceSquared(origin, a);
                    var db = metric.DistanceSquared(origin, b);

                    return da.CompareTo(db) <= 0 ? a : b;
                }

                // その他、距離空間に対するアルゴリズムをこの中に書く
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // 使う側に関してはだいぶ短く書けた
            var n = FloatPoint._2.Euclidean.Nearest(
                (0, 0), (1, 2), (3, 4));

            Console.WriteLine((n.Item1, n.Item2));
        }
    }
}
