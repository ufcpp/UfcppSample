using System;

namespace MetricSpace
{
    public struct ChebyshevMetric<T, TArray, TArrayAccessor, TArithmetic> : IMetric<T, TArray>
        where T : IComparable<T>
        where TArray : struct, IFixedArray<T>
        where TArrayAccessor : struct, IFixedArrayAccessor<T, TArray>
        where TArithmetic : struct, IArithmetic<T>
    {
        public T DistanceSquared(TArray a, TArray b)
        {
            var accessor = default(TArrayAccessor);
            var arithmetic = default(TArithmetic);

            T distance = arithmetic.Zero;
            var dim = accessor.Length;

            for (var i = 0; i < dim; i++)
            {
                T distOnThisAxis = arithmetic.Subtract(accessor.At(ref a, i), accessor.At(ref b, i));
                distance = Max(distance, Abs(distOnThisAxis));
            }

            return arithmetic.Multiply(distance, distance);
        }

        private static T Abs(T x)
        {
            var arithmetic = default(TArithmetic);
            if (x.CompareTo(arithmetic.Zero) < 0) return arithmetic.Subtract(arithmetic.Zero, x);
            return x;
        }

        private static T Max(T x, T y) => x.CompareTo(y) >= 0 ? x : y;
    }
}
