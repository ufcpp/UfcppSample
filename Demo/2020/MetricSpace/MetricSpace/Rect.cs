namespace MetricSpace
{
    public partial class Point<T, TArithmetic>
    {
        public partial class Dimension<TArray, TArrayAccessor>
        {
            public struct Rect
            {
                public TArray MinPoint;
                public TArray MaxPoint;

                public Rect(TArray minPoint, TArray maxPoint) => (MinPoint, MaxPoint) = (minPoint, maxPoint);

                private static readonly int Dimension = default(TArrayAccessor).Length;

                static Rect()
                {
                    var accessor = default(TArrayAccessor);
                    var arithmetic = default(TArithmetic);
                    var dim = Dimension;

                    var rect = new Rect();

                    for (int i = 0; i < dim; i++)
                    {
                        accessor.At(ref rect.MinPoint, i) = arithmetic.NegativeInfinity;
                        accessor.At(ref rect.MaxPoint, i) = arithmetic.PositiveInfinity;
                    }

                    Infinite = rect;
                }

                public static readonly Rect Infinite;

                public TArray GetClosestPoint(TArray toPoint)
                {
                    var accessor = default(TArrayAccessor);
                    var dim = Dimension;
                    TArray closest = default;

                    for (var dimension = 0; dimension < dim; dimension++)
                    {
                        var min = accessor.At(ref MinPoint, dimension);
                        var max = accessor.At(ref MaxPoint, dimension);
                        var to = accessor.At(ref toPoint, dimension);
                        ref var c = ref accessor.At(ref closest, dimension);

                        if (min.CompareTo(to) > 0)
                        {
                            c = min;
                        }
                        else if (max.CompareTo(to) < 0)
                        {
                            c = max;
                        }
                        else
                            // Point is within rectangle, at least on this dimension
                            c = to;
                    }

                    return closest;
                }

                public void Intersect(Rect rect)
                {
                    var accessor = default(TArrayAccessor);
                    var dim = Dimension;

                    for (var dimension = 0; dimension < dim; dimension++)
                    {
                        ref var min = ref accessor.At(ref MinPoint, dimension);
                        var rMin = accessor.At(ref rect.MinPoint, dimension);
                        if (min.CompareTo(rMin) < 0) min = rMin;

                        ref var max = ref accessor.At(ref MaxPoint, dimension);
                        var rMax = accessor.At(ref rect.MaxPoint, dimension);
                        if (max.CompareTo(rMax) > 0) max = rMax;
                    }
                }

                public bool Intersects(Rect rect)
                {
                    var accessor = default(TArrayAccessor);
                    var dim = Dimension;

                    for (var dimension = 0; dimension < dim; dimension++)
                    {
                        if (accessor.At(ref MaxPoint, dimension).CompareTo(accessor.At(ref rect.MinPoint, dimension)) < 0) return false;
                        if (accessor.At(ref rect.MaxPoint, dimension).CompareTo(accessor.At(ref MinPoint, dimension)) < 0) return false;
                    }

                    return true;
                }

                public bool Contains(TArray point)
                {
                    var accessor = default(TArrayAccessor);
                    var dim = Dimension;

                    for (var dimension = 0; dimension < dim; dimension++)
                    {
                        if (accessor.At(ref MinPoint, dimension).CompareTo(accessor.At(ref point, dimension)) > 0) return false;
                        if (accessor.At(ref MaxPoint, dimension).CompareTo(accessor.At(ref point, dimension)) < 0) return false;
                    }

                    return true;
                }

                public override string ToString() => $"{MinPoint}-{MaxPoint}";
            }
        }
    }
}
