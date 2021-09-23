using System.Globalization;
using System.Runtime.CompilerServices;

// Format(DummyHandler) を呼んでて、
// new DummyHandler(5, 2) が作られる。
Formatter.Format($"abc {1} {2}");

// Format(IFormatProvider, DummyHandler) を呼んでて、
// new DummyHandler(5, 2, CultureInfo.InvariantCulture) が作られる。
Formatter.Format(CultureInfo.InvariantCulture, $"abc {1} {2}");

// Format(IFormatProvider, Span<char>, DummyHandler) を呼んでて、
// new DummyHandler(5, 2, CultureInfo.InvariantCulture, stackalloc char[128]) が作られる。
Formatter.Format(CultureInfo.InvariantCulture, stackalloc char[128], $"abc {1} {2}");

[InterpolatedStringHandler]
public ref struct DummyHandler
{
    public int LiteralLength { get; }
    public int FormattedCount { get; }
    public IFormatProvider? Provider {  get; }
    public Span<char> InitialBuffer { get; }

    public DummyHandler(int literalLength, int formattedCount) : this(literalLength, formattedCount, null, default) { }

    // 追加の引数持ち
    public DummyHandler(int literalLength, int formattedCount, IFormatProvider? provider)
        : this(literalLength, formattedCount, provider, default) { }

    public DummyHandler(int literalLength, int formattedCount, IFormatProvider? provider, Span<char> initialBuffer)
    {
        LiteralLength = literalLength;
        FormattedCount = formattedCount;
        Provider = provider;
        InitialBuffer = initialBuffer;
    }

    public void AppendLiteral(string s) { }
    public void AppendFormatted<T>(T x) { }
}

public class Formatter
{
    // 追加の引数なし。
    public static void Format(DummyHandler handler)
        => Console.WriteLine($@"----
{handler.LiteralLength}
{handler.FormattedCount}
{(handler.Provider as CultureInfo)?.DisplayName ?? "(null)"}
{handler.InitialBuffer.Length}
");

    // provider を追加。
    public static void Format(
        IFormatProvider provider,
        [InterpolatedStringHandlerArgument("provider")] DummyHandler handler)
        => Format(handler); // 処理自体は追加引数なしのやつに丸投げ。

    // provider と initialBuffer を追加。
    public static void Format(
        IFormatProvider provider, Span<char> initialBuffer,
        [InterpolatedStringHandlerArgument("provider", "initialBuffer")] DummyHandler handler)
        => Format(handler); // 処理自体は追加引数なしのやつに丸投げ。
}
