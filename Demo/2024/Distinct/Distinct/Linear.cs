namespace Distinct;

public class Linear
{
    public static ReadOnlySpan<T> Distinct<T>(IEnumerable<T> source, Span<T> buffer)
        where T : IEquatable<T>
    {
        var count = 0;

        foreach (var item in source)
        {
            if (buffer[..count].Contains(item)) continue;
            buffer[count++] = item;
        }

        return buffer[..count];
    }
}
