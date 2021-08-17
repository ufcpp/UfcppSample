using System.Globalization;

var x = 1234;
var y = 1.234;
var z = new DateOnly(2001, 2, 3);

// 既存の構文のまま、コンパイル結果が変わるみたい。
// - DefaultInterpolatedStringHandler 型があるかどうかで分岐してそう
//   - この場合、コンパイル結果は後述の DefaultInterpolatedStringHandler への代入 → ToStringAndClear と一緒。
// - 再コンパイル必須(再コンパイルしないと string.Format 呼び出しのまま)
// - ちなみに、CurrentCulture を参照
string s = $"{x} / {y} / {z}";
Console.WriteLine(s);

// DefaultInterpolatedStringHandler などを始めとする
// 所定のパターン (AppendLiteral, AppendFormatted メソッドを持ってる) を満たす型への代入すると、
// AppendLiteral, AppendFormatted メソッド呼び出しに展開される。
System.Runtime.CompilerServices.DefaultInterpolatedStringHandler h = $"{x} / {y} / {z}";
Console.WriteLine(h.ToStringAndClear());

// 上記コードは以下のようなコードとほぼ一緒。
h = new();
h.AppendFormatted(x);
h.AppendLiteral(" / ");
h.AppendFormatted(y);
h.AppendLiteral(" / ");
h.AppendFormatted(z);
Console.WriteLine(h.ToStringAndClear());

// ちなみに、 DefaultInterpolatedStringHandler は ArrayPool から配列を借り出してるので、
// 最後に ToStringAndClear を呼ばないと Pool への変換処理が掛からなくてまずい。
// この罠(マジでメモリリーク)があるので、DefaultInterpolatedStringHandler はあんまり直接触られたくはなさそう。
// ほとんどの場合、string s = $"" で事足りるし、細かいカスタマイズも下記の string.Create でできるはず。

// カルチャー指定。
// string.Create(IFormatProvider, DefaultInterpolatedStringHandler) が呼ばれてる。
var culture = new CultureInfo("fr-fr");
s = string.Create(culture, $"{x} / {y} / {z}");
Console.WriteLine(s);

// 初期バッファー渡す。
// string.Create(IFormatProvider, Span<char>, DefaultInterpolatedStringHandler) が呼ばれてる。
// 最速を目指すならその string.Create 多用することになるはず。
// (InvariantCulture と CurrentCulture でもパフォーマンス変わるかも？)
culture = CultureInfo.InvariantCulture;
s = string.Create(culture, stackalloc char[512], $"{x} / {y} / {z}");
Console.WriteLine(s);

// ↑の例で、IFormatProvider, Span<char> が DefaultInterpolatedStringHandler に渡すのに、
// InterpolatedStringHandlerArgument 属性が使われてる。

// DummyHandler の方が呼ばれる。
C.M($"{x} / {y} / {z}");

// これが string の方になるのはいいとして…
// (M(string) がないとコンパイル エラー。)
C.M("");

// この2つも string の方になる。
// 評価結果が const string になっちゃう $"" はただの string 扱い。
// (ただし、M(string) がないと M(DummyHandler) の方に行く。)
C.M($"");
C.M($"{"abc"}");

class C
{
    public static void M(string _) => Console.WriteLine("string");
    public static void M(DummyHandler _) => Console.WriteLine("DummyHandler");
}

// これが $"" を受け取れる最低ラインのパターン。
[System.Runtime.CompilerServices.InterpolatedStringHandler]
public struct DummyHandler
{
    public DummyHandler(int literalLength, int formattedCount) { }
    // 追加で、任意の引数を足して、InterpolatedStringHandlerArgument 属性を介して受け取れる。
    // あと、末尾に out bool 引数を足せば「以降の AppendLiteral/AppendFormatted は一切呼ばない」みたいな分岐もできる。

    public void AppendLiteral(string s) { }
    public void AppendFormatted<T>(T x, int alignment = 0, string? format = null) { }
    // alignment, format 引数はなくてもいい。
    // これらの引数がない場合、単に $"{value: X, 4}" みたいなのがコンパイル エラーになるだけ。
    // 戻り値を bool にして、false を返すとそれ以降の Append は呼ばないみたいな分岐もできる。
}