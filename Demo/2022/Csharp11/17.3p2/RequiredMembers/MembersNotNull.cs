#pragma warning disable CS8618

//### 非 null 参照型フィールド/プロパティ
class MembersNotNull
{
    // C# 8.0 で null 共用参照型が入って以来、この X, Y に出てくる警告を真っ当に消す手段がなかった。
    // 非 null な参照型のメンバーはコンストラクター内で初期化されないといけない。
    // (このソースコードでは pragma で抑止してる。真っ当に消せるはずの警告も一緒に消えちゃったりするんであんまりよくない。)
    //
    // もちろん、コンストラクターを用意すれば解消はするものの…
    // C# 9.0 で init の意味がちょっと薄れる。
    public string X { get; init; }
    public string Y { get; init; }
}

//### positional 初期化

// 「コンストラクターを用意すれば解決とは言ったものの…
// record がない頃はコンストラクターを書くのも結構面倒。

class InitWithConstructor
{
    // x, y の文字をそれぞれ4回書く必要あり。
    public string X { get; init; }
    public string Y { get; init; }
    public InitWithConstructor(string x, string y)
    {
        X = x;
        Y = y;
    }

    // さらに言うと、後からプロパティを足そうとすると…
    public string Z { get; init; }

    // コンストラクターももう1回書き直し。
    public InitWithConstructor(string x, string y, string z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

// record にはプライマリ コンストラクターっていう仕様があって、
// 以下のようなコードから上記のようなプロパティとコンストラクターを生成してくれる。
// これで多少面倒は減ったものの…
record PositionalBase(string X, string Y);

// 派生クラスを作ったときに X, Y をどこかから伝搬しないといけない。
// だいたいは以下のように X, Y を再度書く必要あり。
record PositionalDerived(string X, string Y, string Z) : PositionalBase(X, Y);

//### nominal 初期化
//
// コンストラクター引数(順序に意味があるので「positional (位置による) 初期化」という)だから追加・拡張が面倒なので…
// プロパティ初期化(名前に意味があるので「nominal (名前による)初期化」という)が欲しくなったり。

record NominalBase
{
    public string X { get; init; }
    public string Y { get; init; }
}

record NominalDerived : NominalBase
{
    // X, Y の伝搬不要！
    public string Z { get; init; }
}

// そして冒頭の問題に戻る。
// NominalBase に出てる「非 null 参照型プロパティはちゃんと初期化しろ」警告が消せない。 
