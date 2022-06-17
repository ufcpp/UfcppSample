#pragma warning disable CS8321

//## nameof(parameter)
//
// メソッド引数に対して nameof(parameter) するときのスコープがちょっと変更。
// * C# 10 まで: メソッドとか他の引数に付けた属性はスコープ外
// * C# 11 以降: メソッドとか他の引数に付けた属性もスコープに入る

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

string s = m("");
Console.WriteLine(s);

// 今までこの属性、 NotNullIfNotNull("x") と書かないといけなくて割かしつらかった。
[return: NotNullIfNotNull(nameof(x))]
static string? m(string? x) => x;

// const string iterpolation と組み合わせれば…
[My($"""
    この文字列を Source Generator とかで使うとして、引数名を参照したいときに
    {nameof(x)}, {nameof(y)}
    こんな感じで引数の名前を埋め込める。
    """)]
void gen(int x, string y) { }

// string.Create の引数はこんな感じ:
static string Create(
    IFormatProvider? provider,
    Span<char> initialBuffer,
    // これも C# 11 から OK に。
    [InterpolatedStringHandlerArgument(nameof(provider), nameof(initialBuffer))]
    ref DefaultInterpolatedStringHandler handler)
{
    return "";
}

// ArgumentNullException.ThrowIfNull の引数はこんな感じ:
static void ThrowIfNull(
    [NotNull] object? argument,
    // この nameof(argument) も C# 11 から。
    [CallerArgumentExpression(nameof(argument))] string? paramName = null)
{ }

class MyAttribute : Attribute
{
    public MyAttribute(string message) { }
}
