//## シフト演算子の右辺の制限緩和
//
// C# のシフト演算子のオーバーロード、これまでは「右オペランドは int でないとダメ」という制限が掛かってた。
//
// 思想的な問題で、C# は「組み込み型に対する使い方と著しく乖離した演算子オーバーロードはあんまり認めたくない」という感じ。
// (「string の + の concat はいいのか」という批判はあり。
//  少なくとも、C++ の cout << "" << endl; はさせたくない。)
//
// これも、generic math に伴って制限緩和が掛かることに。

//### 背景
//
// 背景は Background.cs を参照。

//### 制限緩和

// 4ビット整数(0～15)みたいなのを作ったとして、
public struct Int4Bit
{
    public byte Value { get; }
    public Int4Bit(int value) => Value = (byte)(0xF & value);

    // シフト演算の右オペランドも「自身の型」でやれるように。
    // (前までは int y しかダメだった。)
    public static Int4Bit operator <<(Int4Bit x, Int4Bit y) => new(x.Value << y.Value);
    public static Int4Bit operator >>(Int4Bit x, Int4Bit y) => new(x.Value >> y.Value);
}

//### 欠点
//
// 冒頭で「させたくない」と書いた cout << "" << endl; ができちゃう。
// Drawback.cs 参照。
//
// 言語的に禁止しなくても、ガイドライン等整備して非推奨であることを伝えれば十分ではないかという感じの流れ。
