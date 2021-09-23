DummyHandler h = $"{123}, {"abc"}, {stackalloc char[1]}";

[System.Runtime.CompilerServices.InterpolatedStringHandler]
public struct DummyHandler
{
    public DummyHandler(int literalLength, int formattedCount) { }
    public void AppendLiteral(string s) => Console.WriteLine("(literal)");
    public void AppendFormatted<T>(T x) => Console.WriteLine("ジェネリック版");
    public void AppendFormatted(string x) => Console.WriteLine("string 版");
    public void AppendFormatted(ReadOnlySpan<char> x) => Console.WriteLine("ReadOnlySpan 版");
}
