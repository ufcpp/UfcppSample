using System;

namespace MetricSpace.Devirtualized3
{
    struct ChebyshevMetric<T, TArithmetic, TArray, TArrayAccessor> : IMetric<T, TArray>
        where T : IComparable<T>
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
                d = Max(d, Abs(dif));
            }
            return arith.Multiply(d, d);
        }

        private static T Abs(T x)
        {
            var arith = default(TArithmetic);
            if (x.CompareTo(arith.Zero) < 0) return arith.Subtract(arith.Zero, x);
            return x;
        }

        private static T Max(T x, T y) => x.CompareTo(y) >= 0 ? x : y;
    }
}
