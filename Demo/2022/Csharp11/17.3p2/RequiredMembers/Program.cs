//## Required メンバー
//
// C# 8.0 で null 許容参照型が入って以来ずっとある課題。
// 非 null な参照型フィールド/プロパティがあると、コンストラクターが必須になっちゃう。
// コンストラクターを用意するのが結構めんどくさい場面も多々あってしんどかった。
// MembersNotNull.cs を参照。

using System.Diagnostics.CodeAnalysis;

//### required 修飾子
//
// そこで C# 11 で導入されるのが required。
// プロパティ(とか、public フィールド)に付けておくと、オブジェクト初期化子で初期化することを義務付けられるようになった。

var a = new A
{
    // ここの2行、どちらか片方でも削るとコンパイル エラーになる。
    X = "",
    Y = 1,
};

Console.WriteLine(a);

record A
{
    // required を付けると初期化子での初期化が必須になる。
    public required string X { get; init; }
    public required int Y { get; init; }

    public A() { }

    // ちなみに、「コンストラクターでちゃんと初期化してるから、初期化子で改めて値を渡す必要はないよ」みたいな実装をしたい場合、
    // SetsRequiredMembers 属性を付ける。
    // こっちなら var a = new A("", 1); とか書ける。
    [SetsRequiredMembers]
    public A(string x, int y)
    {
        X = x;
        Y = y;

        // ただし注意が2点:
        // * 「X だけは初期化したけど、Y は初期化子で渡してくれ」みたいな部分指定はできない
        // * このコンストラクター内で Y に何も代入しなくても特に警告は出ない。実装者任せ
    }

    // 未対応のバージョンのコンパイラーで new A(); だけ呼ばれると困るので、
    // A() の方には「対応していなければエラーになる属性」みたいなのを付けるみたい。
    // (今のところ Obsolete 属性を使ってやってる。)
}

// ちなみに、virtual にしたりできる。
// (abstract も可。)
class Base
{
    public virtual required string X { get; set; }
}

class Derived : Base
{
    // 派生側で required を外すのはダメ。コンパイル エラーに。
    //public override string X { get; init; }

    // これなら OK。
    public override required string X { get; set; }
}
