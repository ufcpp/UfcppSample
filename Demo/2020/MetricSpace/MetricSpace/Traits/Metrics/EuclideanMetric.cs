namespace MetricSpace
{
    public struct EuclideanMetric<T, TArray, TArrayAccessor, TArithmetic> : IMetric<T, TArray>
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
                T distOnThisAxisSquared = arithmetic.Multiply(distOnThisAxis, distOnThisAxis);
                distance = arithmetic.Add(distance, distOnThisAxisSquared);
            }

            return distance;
        }
    }
}
