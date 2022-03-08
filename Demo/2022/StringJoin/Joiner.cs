using System.Text;

namespace StringJoin;

public class Joiner
{
    public static Joiner<T> Join<T>(string separator, IEnumerable<T> items)
        where T : ISpanFormattable
        => new(separator, items);
}

public struct Joiner<T> : ISpanFormattable
    where T : ISpanFormattable
{
    private readonly string _separator;
    private readonly IEnumerable<T> _items;
    public Joiner(string separator, IEnumerable<T> items) => (_separator, _items) = (separator, items);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var first = true;
        charsWritten = 0;
        foreach (var item in _items)
        {
            if (first) first = false;
            else
            {
                if (destination.Length < _separator.Length)
                {
                    return false;
                }
                _separator.CopyTo(destination);
                charsWritten += _separator.Length;
                destination = destination[_separator.Length..];
            }

            if (!item.TryFormat(destination, out var w, format, provider)) return false;

            charsWritten += w;
            destination = destination[w..];
        }
        return true;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var s = new StringBuilder();
        var first = true;
        foreach (var item in _items)
        {
            if (first) first = false;
            else s.Append(_separator);
            s.Append(item.ToString(format, formatProvider));
        }
        return s.ToString();
    }

    public override string ToString() => ToString(null, null);
}
