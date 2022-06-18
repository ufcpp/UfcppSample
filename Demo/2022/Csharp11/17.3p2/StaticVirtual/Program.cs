//## static virtual
//
// インターフェイスの static abstract メソッドに対して実装を持てるようになった。
// といっても、そもそも、static abstract 自体まだ例を挙げていないのでそこから。
//
// (一応、去年、preview のころにちょこっとだけサンプルを書いてる:
//  https://github.com/ufcpp/UfcppSample/tree/master/Demo/2021/Csharp10/StaticAbstract/GenericMath/Algebra )

//### static abstract
//
// インターフェイスの静的メンバーを abstract にできるようになった。
// StaticAbstract.cs を参照。

StaticAbstract.M();

// 一番の用途は generic math。
// GenericMath.cs を参照。

GenericMath.M();

// 注意点として、static abstract なメンバーは、全然動的な挙動をしない。
// StaticBehavior.cs を参照。

StaticBehavior.M();

//### static virtual
//
// C# 8.0 で、インターフェイスにデフォルト実装を持てるようになってるわけで。
// https://ufcpp.net/study/csharp/oo_interface.html?p=5#dim
//
// 今回正式に入る(C# 10 の頃に preview リリースされた) static abstract メンバーで、デフォルト実装を持てない理由もない。
// ただ、まあ、17.3p2 でようやく実装されたとのこと。
//
// 文法的に static virtual って書くのでそのまま「static virtual メンバー」と言ったり、
// デフォルト実装が DIM (default interface method) という呼ばれ方もあるので「DIM for static abstract」と言ったり。

m(new Int4Bit(6), 4);

static void m<T>(T start, int count)
    where T : IAdditive<T>
{
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine(start);
        ++start;
    }
}

public interface IAdditive<T> where T : IAdditive<T>
{
    static abstract T One { get; }
    static abstract T operator+(T a, T b);

    // これが今回から可能に(static virtual)。
    // インクリメントとか、+ 演算子と One があれば汎用に実装可能。
    static virtual T operator ++(T a) => a + T.One;
}

public struct Int4Bit : IAdditive<Int4Bit>
{
    public byte Value { get; }
    public Int4Bit(int value) => Value = (byte)(0xF & value);
    public override string ToString() => Value.ToString();

    // static abstract を実装。
    public static Int4Bit One => new(1);
    public static Int4Bit operator +(Int4Bit a, Int4Bit b) => new(a.Value + b.Value);

    // デフォルト実装で十分なら ↑ 3行で終わりだし、
    // もし、別の実装を持ちたければ、以下のような行を足してもいい。
    //public static Int4Bit operator ++(Int4Bit a) => new(a.Value + 1);
}
