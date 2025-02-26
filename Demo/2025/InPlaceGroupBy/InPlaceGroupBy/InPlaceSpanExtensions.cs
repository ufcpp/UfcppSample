namespace InPlaceGroupBy;

public static class InPlaceSpanExtensions
{
    public static SortedSpanGrouping<T> GroupBy<T>(this Span<T> span, Comparison<T> comparison)
    {
        span.Sort(comparison);
        return new(span, comparison);
    }
}
