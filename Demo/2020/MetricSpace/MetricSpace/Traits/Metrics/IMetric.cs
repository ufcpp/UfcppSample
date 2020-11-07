using System.Collections.Generic;

namespace MetricSpace
{
    public interface IMetric<T, TArray>
        where TArray : IFixedArray<T>
    {
        T DistanceSquared(TArray a, TArray b);
    }
}
