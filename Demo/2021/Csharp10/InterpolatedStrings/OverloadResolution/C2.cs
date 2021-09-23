using System.Runtime.CompilerServices;

namespace OverloadResolution;

internal class C2
{
    public static void Caller()
    {
        // 優先度は付かないので不明瞭エラーを起こす。
#if ERROR
        M($"");
#endif

        // 明示的にキャストすれば呼び分け可能。
        M((Handler1)$"");
        M((Handler2)$"");
    }

    public static void M(Handler1 _) => Console.WriteLine("Handler1");
    public static void M(Handler2 _) => Console.WriteLine("Handler2");
}

[InterpolatedStringHandler]
public struct Handler1
{
    public Handler1(int literalLength, int formattedCount) { }
    public void AppendLiteral(string s) { }
    public void AppendFormatted<T>(T x) { }
}

[InterpolatedStringHandler]
public struct Handler2
{
    public Handler2(int literalLength, int formattedCount) { }
    public void AppendLiteral(string s) { }
    public void AppendFormatted<T>(T x) { }
}
