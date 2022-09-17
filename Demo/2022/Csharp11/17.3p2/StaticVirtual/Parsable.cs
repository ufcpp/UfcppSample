using System.Buffers.Text;
using System.Text;

interface IUtf8Parsable<T>
    where T : IUtf8Parsable<T>
{
    // 静的メンバーにしたいもの筆頭が、ファクトリメソッドの類。
    // この例では Parse (文字列から T のインスタンスを作る)にしているものの、
    // 例えば static T Create(); みたいなものの需要も結構高いはず。
    static abstract T Parse(ReadOnlySpan<byte> utf8);

    // virtual にもできる。
    // デフォルト実装を持ちつつ、必要であればクラス側で別実装を書ける。
    public static virtual T Parse(string s)
    {
        var buffer = (stackalloc byte[s.Length]);
        var read = Encoding.ASCII.GetBytes(s, buffer);
        return T.Parse(buffer[..read]);
    }
}

// 実装例:
record struct Point(int X, int Y) : IUtf8Parsable<Point>
{
    public static Point Parse(ReadOnlySpan<byte> utf8)
    {
        var i = utf8.IndexOf((byte)',');
        var xs = utf8[..i];
        var ys = utf8[(i + 1)..];

        Utf8Parser.TryParse(xs, out int x, out _);
        Utf8Parser.TryParse(ys, out int y, out _);

        return new(x, y);
    }
}
