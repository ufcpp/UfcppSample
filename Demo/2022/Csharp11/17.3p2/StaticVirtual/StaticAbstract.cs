// インターフェイスの静的メンバーを abstract にできるように。
// というか、C# 10 の頃から「preview feature」として提供されてた。
// (当時は、EnablePreviewFeatures オプションを立てないと使えない状態。)
// これも C# 11 で正式リリースで、オプション指定なしで使えるようになる。

using System.Buffers.Binary;

class StaticAbstract
{
    public static void M()
    {
        var a = new MyData { X = 1, Y = 2, Z = 3 };
        var b = a.BinaryCopy();

        a.X = 10;

        Console.WriteLine(a);
        Console.WriteLine(b);
    }
}

// インターフェイスに static abstract なメンバーを定義する例。
interface ISerializable<TSelf>
{
    static abstract int RequiedBufferSize { get; }
    void Format(Span<byte> destination);

    // static でないと、この手のインスタンス新規作成系のメソッドを定義しにくかった。
    // これまでもファクトリ インターフェイスを別途用意すれば出来はしたものの、
    // TSelf に加えて TFactory みたいな別の型引数を追加で渡さないといけなかったり、結構面倒だった。
    static abstract TSelf Parse(ReadOnlySpan<byte> source);
}

// 利用例
static class Serializable
{
    public static TSelf BinaryCopy<TSelf>(this TSelf x)
        where TSelf : ISerializable<TSelf>
    {
        // こことか
        var buffer = (stackalloc byte[TSelf.RequiedBufferSize]);
        x.Format(buffer);

        // ここみたいに、型引数.メンバー名 で static abstract メンバーを参照できる。
        return TSelf.Parse(buffer);
    }
}

// インターフェイスの実装例。
class MyData : ISerializable<MyData>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public override string ToString() => $"{X}, {Y}, {Z}";

    public static int RequiedBufferSize => 12;

    public void Format(Span<byte> destination)
    {
        BinaryPrimitives.WriteInt32LittleEndian(destination, X);
        BinaryPrimitives.WriteInt32LittleEndian(destination[4..], Y);
        BinaryPrimitives.WriteInt32LittleEndian(destination[8..], Z);
    }

    public static MyData Parse(ReadOnlySpan<byte> source)
    {
        return new()
        {
            X = BinaryPrimitives.ReadInt32LittleEndian(source),
            Y = BinaryPrimitives.ReadInt32LittleEndian(source[4..]),
            Z = BinaryPrimitives.ReadInt32LittleEndian(source[8..]),
        };
    }
}
