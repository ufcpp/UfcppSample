namespace MetricSpace
{
    public interface IArithmetic<T>
    {
        T MinValue { get; }
        T MaxValue { get; }
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Zero { get; }
        T NegativeInfinity { get; }
        T PositiveInfinity { get; }
    }
}
