namespace Distinct;

public class Binary
{
    public static ReadOnlySpan<T> Distinct<T>(IEnumerable<T> source, Span<T> buffer)
        where T : IComparable<T>
    {
        var count = 0;

        foreach (var item in source)
        {
            var i = buffer[..count].BinarySearch(item);
            if (i >= 0) continue;
            InsertAt(buffer, ~i, item);
            ++count;
        }

        return buffer[..count];
    }

    private static void InsertAt<T>(Span<T> span, int index, T value)
    {
        span[index..^1].CopyTo(span[(index + 1)..]);
        span[index] = value;
    }
}

/*
    public static int BinarySearch<TItem, TSearch>(this IList<TItem> list, TSearch value, Func<TSearch, TItem, int> comparer)
    {
        ThrowHelper.ThrowIfNull(list);
        ThrowHelper.ThrowIfNull(comparer);

        int lower = 0;
        int upper = list.Count - 1;

        while (lower <= upper)
        {
            int middle = lower + (upper - lower) / 2;
            int comparisonResult = comparer(value, list[middle]);
            if (comparisonResult < 0)
            {
                upper = middle - 1;
            }
            else if (comparisonResult > 0)
            {
                lower = middle + 1;
            }
            else
            {
                return middle;
            }
        }

        return ~lower;
    }
 * */
