m(1, 2);
m1(1, 2);

void m(int a, int b)
{
    DummyHandler h = $"{a} / {b}";
}

// m の展開結果
void m1(int a, int b)
{
    DummyHandler temp = new(3, 2);
    temp.AppendFormatted(a);
    temp.AppendLiteral(" / ");
    temp.AppendFormatted(b);
    DummyHandler h = temp;
}

[System.Runtime.CompilerServices.InterpolatedStringHandler]
public struct DummyHandler
{
    public DummyHandler(int literalLength, int formattedCount) { }
    public void AppendLiteral(string s) { }
    public void AppendFormatted<T>(T x) { }
}
