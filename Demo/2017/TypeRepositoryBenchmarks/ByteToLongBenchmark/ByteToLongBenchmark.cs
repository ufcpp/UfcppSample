using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

/// <summary>
/// byte 配列を long にして8バイトずつ読みだす処理のベンチマーク。
/// byte 配列長が8の倍数でない場合、最後の部分は0詰め(little endianだとlongの上位桁が0)。
///
/// 目的が文字列の UTF8 エンコード結果の比較なので、0はないはず(null文字には対応しない前提) → 0 詰めしても平気。
///
///             Method |     Mean |     Error |    StdDev |
/// ------------------ |---------:|----------:|----------:|
///   PointerBenchmark | 3.739 us | 0.0504 us | 0.0447 us |
///  Pointer2Benchmark | 3.631 us | 0.0482 us | 0.0451 us |
///      SpanBenchmark | 4.083 us | 0.0454 us | 0.0402 us |
///
/// ほんとにパフォーマンス最優先なところは unsafe 使いたい感じはある。
/// </summary>
public class ByteToLongBenchmark
{
    byte[] rawData;

    [GlobalSetup]
    public void Setup()
    {
        rawData = new byte[256];
        for (int i = 0; i < 256; i++) rawData[i] = (byte)i;
    }

    const int N = 32;

    [Benchmark]
    public unsafe ulong PointerBenchmark()
    {
        var r = new Random();
        var sum = 0UL;

        for (int itr = 0; itr < N; itr++)
        {
            var offset = r.Next(0, 256);
            var length = r.Next(1, 256 - offset);

            fixed (byte* p = rawData)
            {
                var x = p + offset;

                while (MoveNext(ref x, ref length, out var v))
                {
                    sum += v;
                }
            }
        }
        return sum;
    }

    [Benchmark]
    public unsafe ulong Pointer2Benchmark()
    {
        var r = new Random();
        var sum = 0UL;

        for (int itr = 0; itr < N; itr++)
        {
            var offset = r.Next(0, 256);
            var length = r.Next(1, 256 - offset);

            fixed (byte* p = rawData)
            {
                var x = p + offset;

                while (MoveNext2(ref x, ref length, out var v))
                {
                    sum += v;
                }
            }
        }
        return sum;
    }

    [Benchmark]
    public ulong SpanBenchmark()
    {
        var r = new Random();
        var sum = 0UL;

        for (int itr = 0; itr < N; itr++)
        {
            var offset = r.Next(0, 256);
            var length = r.Next(1, 256 - offset);

            ReadOnlySpan<byte> wholeSpan = rawData;
            var x = wholeSpan.Slice(offset, length);

            while (MoveNext(ref x, out var v))
            {
                sum += v;
            }
        }
        return sum;
    }

    /// <summary>
    /// <see cref="Span{T}"/>版。
    /// </summary>
    public static bool MoveNext(ref ReadOnlySpan<byte> x, out ulong value)
    {
        var len = Math.Min(x.Length, 8);
        switch (len)
        {
            default:
            case 8:
                value = x.NonPortableCast<byte, ulong>()[0];
                break;
            case 0:
                value = 0;
                return false;
            case 1:
                value = x[0];
                break;
            case 2:
                value = x.NonPortableCast<byte, ushort>()[0];
                break;
            case 3:
                value = x.NonPortableCast<byte, ushort>()[0] | ((ulong)x[2] << 16);
                break;
            case 4:
                value = x.NonPortableCast<byte, uint>()[0];
                break;
            case 5:
                value = x.NonPortableCast<byte, uint>()[0] | ((ulong)x[4] << 32);
                break;
            case 6:
                value = x.NonPortableCast<byte, uint>()[0] | ((ulong)x.NonPortableCast<byte, ushort>()[2] << 32);
                break;
            case 7:
                value = x.NonPortableCast<byte, uint>()[0] | ((ulong)x.NonPortableCast<byte, ushort>()[2] << 32) | ((ulong)x[6] << 48);
                break;
        }

        x = x.Slice(len);
        return true;
    }

    /// <summary>
    /// ポインター版。
    /// </summary>
    public static unsafe bool MoveNext(ref byte* x, ref int length, out ulong value)
    {
        var len = Math.Min(length, 8);
        switch (len)
        {
            default:
            case 8:
                value = *(ulong*)x;
                break;
            case 0:
                value = 0;
                return false;
            case 1:
                value = *x;
                break;
            case 2:
                value = *((ushort*)x);
                break;
            case 3:
                value = *((ushort*)x) | ((ulong)x[2] << 16);
                break;
            case 4:
                value = *((uint*)x);
                break;
            case 5:
                value = *((uint*)x) | ((ulong)x[4] << 32);
                break;
            case 6:
                value = *((uint*)x) | ((ulong)(*(ushort*)(x + 4)) << 32);
                break;
            case 7:
                value = *((uint*)x) | ((ulong)(*(ushort*)(x + 4)) << 32) | ((ulong)*(x + 6) << 48);
                break;
        }

        x += len;
        length -= len;
        return true;
    }

    /// <summary>
    /// 速いけど unsafe 必須のやり方。
    ///
    /// 末尾部分、8の倍数になってなくてもお構いなしにいったんはみ出して読んで、後でマスクを掛けてはみ出し部分を0クリアする。
    /// いったんはみ出すのが<see cref="Span{T}"/>(ちゃんと範囲チェックしてる)ではできない。
    /// </summary>
    static unsafe bool MoveNext2(ref byte* x, ref int length, out ulong value)
    {
        if (length == 0)
        {
            value = default;
            return false;
        }

        var len = Math.Min(length, 8);
        var mask = unchecked((1UL << (len * 8)) - 1);

        value = *(ulong*)x & mask;

        x += len;
        length -= len;
        return true;
    }

    public unsafe void Test()
    {
        var r = new Random();

        for (int itr = 0; itr < 10; itr++)
        {
            var offset = r.Next(0, 256);
            var length = r.Next(1, 256 - offset);

            fixed (byte* p = rawData)
            {
                ReadOnlySpan<byte> wholeSpan = rawData;
                var x1 = wholeSpan.Slice(offset, length);
                var x2 = p + offset;
                var x3 = p + offset;

                var l2 = length;
                var l3 = length;

                var b1 = MoveNext(ref x1, out var v1);
                var b2 = MoveNext(ref x2, ref l2, out var v2);
                var b3 = MoveNext(ref x3, ref l3, out var v3);

                if (b1 != b2 || b1 != b3) throw new Exception();
                if (v1 != v2 || v1 != v3) throw new Exception();
            }
        }
    }
}