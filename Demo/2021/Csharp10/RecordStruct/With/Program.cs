// with 式の話。

// クラスの場合は通常の class には with 式を使えず、record でだけ使える。
// 通常のクラスには標準的なクローン手法がないので。
// (with 式 = クローン + 部分書き換え)
// (将来的にはパターン ベースで通常のクラスにも with 式を使える手段を提供したいみたいな話もあるけど、少なくとも C# 9, 10 時点ではない。)

//var nc = new NormalClass { X = 1 };
//Console.WriteLine(nc with { X = 2 }); // コンパイルエラー

var rc = new RecordClass(1, 2);
Console.WriteLine(rc with { X = 3 });

// 一方で、構造体の場合は通常の struct でも record struct でもどっちでも with 式を使える。
// (C# 10.0 から。)
// 構造体は元からクローン手段を持ってる(他の変数に代入するだけでコピーが発生してる)。

var ns = new NormalStruct { X = 1, Y = 2 };
Console.WriteLine(ns with { X = 3 });

var rs = new RecordStruct(1, 2);
Console.WriteLine(rs with { X = 3 });

// おまけで、匿名型(C# 3.0 からある参照型のやつ)に対しても、C# 10.0 で with 式が使えるようになった。
// (元から予定はあった。record struct と同じく、スケジュール上 C# 9.0 から外れてただけ。)
var anonymous = new { X = 1, Y = 2 };
Console.WriteLine(anonymous with { X = 3 });

// ちなみに、タプルに対しても with 式が使えるようになってるけども、
// これは理屈としては「構造体は無条件に with 式を使える」の一種。
var tuple = (X: 1, Y: 2);
Console.WriteLine(tuple with { X = 3 });

class NormalClass
{
    public int X { get; init; }
    public int Y { get; init; }
    public override string ToString() => $"NormalClass {{ X = {X}, Y = {Y} }}";
}

record RecordClass(int X, int Y);

struct NormalStruct
{
    public int X { get; init; }
    public int Y { get; init; }
    public override string ToString() => $"NormalStruct {{ X = {X}, Y = {Y} }}";
}

record struct RecordStruct(int X, int Y);

// おまけ:
// 「通常の構造体でも、元々あるクローン機構を使って with 式に対応」という判断とセットで、
// 「手書きで Clone を書いてもそれは呼ばない」ということになり、
// 「呼ばれそうに見えるけど呼ばれない」と言うのが怖いので、「record struct に Clone メソッドは書けない」という縛りを入れたみたい。
#if false
record struct R
{
    public R Clone() => new R(); // コンパイル エラーになる。
}
#endif
