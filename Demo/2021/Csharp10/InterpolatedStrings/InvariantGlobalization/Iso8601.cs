using System.Globalization;
using System.Runtime.CompilerServices;

namespace InvariantGlobalization;

/// <summary>
/// In addition to <see cref="Invariant"/>, use ISO 8601 date format.
/// </summary>
/// <remarks>
/// <see cref="CultureInfo.InvariantCulture"/> uses USA format which is very unique.
///
/// see:
/// <a href="https://en.wikipedia.org/wiki/Calendar_date#Usage_map">Calendar date: Usage map</a>,
/// <a href="https://www.quora.com/Why-do-some-English-speakers-format-their-dates-as-M-D-Y-while-most-of-the-world-uses-either-D-M-Y-or-Y-M-D">Why do some English speakers format their dates as M/D/Y?</a>
/// </remarks>
public static class Iso8601
{
    public static string Format(InterpolatedStringHandler handler) => handler.ToStringAndClear();
    public static string Format(Span<char> initialBuffer, [InterpolatedStringHandlerArgument("initialBuffer")] InterpolatedStringHandler handler) => handler.ToStringAndClear();

    [InterpolatedStringHandler]
    public ref struct InterpolatedStringHandler
    {
        private DefaultInterpolatedStringHandler _inner;

        public InterpolatedStringHandler(int literalLength, int formattedCount) => _inner = new(literalLength, formattedCount, CultureInfo.InvariantCulture);
        public InterpolatedStringHandler(int literalLength, int formattedCount, Span<char> initialBuffer) => _inner = new(literalLength, formattedCount, CultureInfo.InvariantCulture, initialBuffer);

        public void AppendLiteral(string value) => _inner.AppendLiteral(value);
        public void AppendFormatted(ReadOnlySpan<char> value) => _inner.AppendFormatted(value);
        public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        public void AppendFormatted<T>(T value) => _inner.AppendFormatted(value);
        public void AppendFormatted<T>(T value, string? format) => _inner.AppendFormatted(value, format);
        public void AppendFormatted<T>(T value, int alignment) => _inner.AppendFormatted(value, alignment);
        public void AppendFormatted<T>(T value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format);
        public void AppendFormatted(object? value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);
        public void AppendFormatted(string? value) => _inner.AppendFormatted(value);
        public void AppendFormatted(string? value, int alignment = 0, string? format = null) => _inner.AppendFormatted(value, alignment, format);

        public void AppendFormatted(TimeOnly value) => _inner.AppendFormatted(value, "O");
        public void AppendFormatted(TimeOnly value, string? format) => _inner.AppendFormatted(value, format ?? "O");
        public void AppendFormatted(TimeOnly value, int alignment) => _inner.AppendFormatted(value, alignment, "O");
        public void AppendFormatted(TimeOnly value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format ?? "O");

        public void AppendFormatted(DateOnly value) => _inner.AppendFormatted(value, "O");
        public void AppendFormatted(DateOnly value, string? format) => _inner.AppendFormatted(value, format ?? "O");
        public void AppendFormatted(DateOnly value, int alignment) => _inner.AppendFormatted(value, alignment, "O");
        public void AppendFormatted(DateOnly value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format ?? "O");

        public void AppendFormatted(DateTime value) => _inner.AppendFormatted(value, "O");
        public void AppendFormatted(DateTime value, string? format) => _inner.AppendFormatted(value, format ?? "O");
        public void AppendFormatted(DateTime value, int alignment) => _inner.AppendFormatted(value, alignment, "O");
        public void AppendFormatted(DateTime value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format ?? "O");

        public void AppendFormatted(DateTimeOffset value) => _inner.AppendFormatted(value, "O");
        public void AppendFormatted(DateTimeOffset value, string? format) => _inner.AppendFormatted(value, format ?? "O");
        public void AppendFormatted(DateTimeOffset value, int alignment) => _inner.AppendFormatted(value, alignment, "O");
        public void AppendFormatted(DateTimeOffset value, int alignment, string? format) => _inner.AppendFormatted(value, alignment, format ?? "O");

        public string ToStringAndClear() => _inner.ToStringAndClear();
    }
}
