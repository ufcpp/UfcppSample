using System.Numerics;

class Background
{
    public static void M()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"{i} {Log2(i)} {Mask(i)}");
        }
    }

    // こんな感じで「ビット数を調べる」みたいなことはそこそこ出番あり。
    public static int Log2<T>(T x)
        where T : IBinaryInteger<T>
    {
        var count = 0;
        for (; x != T.Zero; x >>>= 1, count++) { }
        return count;
    }

    // その手のビット数、その後シフト演算の右オペランドに使うことあり。
    public static T Mask<T>(T x)
        where T : IBinaryInteger<T>
    {
        var bits = Log2(x);
        return (T.One << bits) - T.One;
    }

    // ↑ビット数、大体は int で十分だけども、戻り値の側も generic math にしたい場合も考えうる。
    public static TBits Log2<T, TBits>(T x)
        where T : IBinaryInteger<T>
        where TBits : INumber<TBits>
    {
        var count = TBits.Zero;
        for (; x != T.Zero; x >>>= 1, count++) { }
        return count;
    }

    // 今、IShiftOperators が operator<< (TSelf, int) しか持っていないので、
    // TBits を介した Mask は実装できない。
}
