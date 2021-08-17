// InterpolatedStringHandler の悪用気味コード。

using System.Runtime.CompilerServices;

var a = 1;
var b = false;
var c = 1.2;
var d = "abc";

// これで
// a: 1
// b: False
// c: 1.2
// d: abc
// になる。
// 濫用気味なトリックとして params Dictionary<string, T> 代わりにも使えたり。
// (ORM の類でなら実用できるかも。)
write<object>($"{a}{b}{c}{d}");

// リテラル直渡ししたときが微妙な挙動だけども…
// 1: 1
// 2: 2
// a: 1
write<int>($"{1}{2}{a}");

static void write<TValue>(ParamsDictionaryHandler<TValue> handler)
{
    var dic = handler.Dictionary;
    foreach (var (key, value) in dic)
    {
        Console.WriteLine($"{key}: {value}");
    }
}

[InterpolatedStringHandler]
public struct ParamsDictionaryHandler<T>
{
    internal readonly Dictionary<string, T> Dictionary;
    public ParamsDictionaryHandler(int _, int formattedCount) => Dictionary = new(formattedCount);
    public void AppendFormatted(T value, [CallerArgumentExpression("value")] string? ex = null)
    {
        if (ex is null) return;
        Dictionary[ex] = value;
    }
}