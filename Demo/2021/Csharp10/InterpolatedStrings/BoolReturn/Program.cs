using System.Runtime.CompilerServices;

Console.WriteLine(m(0x12345678, 0xABCD, 123, 1));

DummyHandler m(int a, int b, int c, int d) => $"{a}.{b}.{c}.{d}";

// m の展開結果
DummyHandler m1(int a, int b, int c, int d)
{
    DummyHandler h = new DummyHandler(3, 4, out var result);
    if (result
        && h.AppendFormatted(a)
        && h.AppendLiteral(".")
        && h.AppendFormatted(b)
        && h.AppendLiteral(".")
        && h.AppendFormatted(c)
        && h.AppendLiteral("."))
        h.AppendFormatted(d);
    return h;
}

[InterpolatedStringHandler]
public struct DummyHandler
{
    public DummyHandler(int literalLength, int formattedCount, out bool result) => result = true;
    public bool AppendLiteral(string s) => true;
    public bool AppendFormatted<T>(T x) => true;
}
