using System.Globalization;
using System.Runtime.CompilerServices;

namespace InvariantGlobalization;

/// <summary>
/// Pass always <see cref="CultureInfo.InvariantCulture"/> to <see cref="DefaultInterpolatedStringHandler"/>.
/// </summary>
public static class Invariant
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
        public string ToStringAndClear() => _inner.ToStringAndClear();
    }
}
