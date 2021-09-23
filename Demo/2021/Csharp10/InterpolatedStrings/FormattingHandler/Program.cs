using System.Runtime.CompilerServices;

Console.WriteLine(m(0x123456, 0xABC, 12));
Console.WriteLine(m1(0x123456, 0xABC, 12));

string m(int a, int b, int c) => $"({a, 8:X}) ({b:X}) ({c,4})";

// m の展開結果
string m1(int a, int b, int c)
{
    DefaultInterpolatedStringHandler h = new(8, 3);
    h.AppendLiteral("(");
    h.AppendFormatted(a, 8, "X");
    h.AppendLiteral(") (");
    h.AppendFormatted(b, "X");
    h.AppendLiteral(") (");
    h.AppendFormatted(c, 4);
    h.AppendLiteral(")");
    return h.ToStringAndClear();
}

DummyHandler m2(int a, int b, int c, int d) => $"({a,8:X}) ({b:X}) ({c,4}) ({d})";

[InterpolatedStringHandler]
public struct DummyHandler
{
    public DummyHandler(int literalLength, int formattedCount) { }
    public void AppendLiteral(string s) { }

    public void AppendFormatted<T>(T x) { }
    public void AppendFormatted<T>(T x, int alignment) { }
    public void AppendFormatted<T>(T x, string format) { }
    public void AppendFormatted<T>(T x, int alignment, string format) { }

    public void AppendFormatted<T>(T x, int? alignment = null, string? format = null) { }
}
