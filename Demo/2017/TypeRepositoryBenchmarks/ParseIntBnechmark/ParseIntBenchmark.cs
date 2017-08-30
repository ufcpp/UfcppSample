using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// UTF8 エンコード結果の byte[] から Int.Parse したいとき用。
/// int.Parse(UTF8.GetString(data))とかアロケーション多すぎなので避けたい。
/// なので byte 列操作を直接やるんだけど、単純に for で回すべきか、展開すべきか。
///
///  Method |     Mean |     Error |    StdDev | Allocated |
/// ------- |---------:|----------:|----------:|----------:|
///       A | 1.357 us | 0.0203 us | 0.0190 us |       0 B |
///       B | 1.470 us | 0.0062 us | 0.0058 us |       0 B |
///       C | 1.373 us | 0.0178 us | 0.0166 us |       0 B |
///       D | 1.289 us | 0.0063 us | 0.0056 us |       0 B |
///       E | 2.728 us | 0.0298 us | 0.0278 us |       0 B |
///
/// この程度の処理なら simple is the best 感強い。
/// </summary>
[MemoryDiagnoser]
public class ParseIntBenchmark
{
    const int N = 128;
    byte[][] data;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random();
        data = new byte[N][];

        for (int i = 0; i < data.Length; i++)
        {
            var x = r.Next();
            data[i] = Encoding.ASCII.GetBytes(x.ToString());
        }
    }

    [Benchmark] public void A() { foreach (var x in data) ParseA(x); }
    [Benchmark] public void B() { foreach (var x in data) ParseB(x); }
    [Benchmark] public void C() { foreach (var x in data) ParseC(x); }
    [Benchmark] public void D() { foreach (var x in data) ParseD(x); }
    [Benchmark] public void E() { foreach (var x in data) ParseE(x); }

    public static int ParseA(byte[] b)
    {
        int x = 0;
        for (int i = 0; i < b.Length; i++)
        {
            x *= 10;
            x += b[i] - Zero;
        }
        return x;
    }

    private const byte Zero = (byte)'0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ParseB(byte[] b)
    {
        switch (b.Length)
        {
            case 1: return (b[0] - Zero);
            case 2: return (b[0] - Zero) * 10 + (b[1] - Zero);
            case 3: return (b[0] - Zero) * 100 + (b[1] - Zero) * 10 + (b[2] - Zero);
            case 4: return (b[0] - Zero) * 1000 + (b[1] - Zero) * 100 + (b[2] - Zero) * 10 + (b[3] - Zero);
            case 5: return (b[0] - Zero) * 10000 + (b[1] - Zero) * 1000 + (b[2] - Zero) * 100 + (b[3] - Zero) * 10 + (b[4] - Zero);
            case 6: return (b[0] - Zero) * 100000 + (b[1] - Zero) * 10000 + (b[2] - Zero) * 1000 + (b[3] - Zero) * 100 + (b[4] - Zero) * 10 + (b[5] - Zero);
            case 7: return (b[0] - Zero) * 1000000 + (b[1] - Zero) * 100000 + (b[2] - Zero) * 10000 + (b[3] - Zero) * 1000 + (b[4] - Zero) * 100 + (b[5] - Zero) * 10 + (b[6] - Zero);
            case 8: return (b[0] - Zero) * 10000000 + (b[1] - Zero) * 1000000 + (b[2] - Zero) * 100000 + (b[3] - Zero) * 10000 + (b[4] - Zero) * 1000 + (b[5] - Zero) * 100 + (b[6] - Zero) * 10 + (b[7] - Zero);
            case 9: return (b[0] - Zero) * 100000000 + (b[1] - Zero) * 10000000 + (b[2] - Zero) * 1000000 + (b[3] - Zero) * 100000 + (b[4] - Zero) * 10000 + (b[5] - Zero) * 1000 + (b[6] - Zero) * 100 + (b[7] - Zero) * 10 + (b[8] - Zero);
            case 10: return (b[0] - Zero) * 1000000000 + (b[1] - Zero) * 100000000 + (b[2] - Zero) * 10000000 + (b[3] - Zero) * 1000000 + (b[4] - Zero) * 100000 + (b[5] - Zero) * 10000 + (b[6] - Zero) * 1000 + (b[7] - Zero) * 100 + (b[8] - Zero) * 10 + (b[9] - Zero);
            default: throw new ArgumentException("Int32 out of range count");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ParseC(byte[] b)
    {
        switch (b.Length)
        {
            case 1: return (b[0] - Zero);
            case 2: return 10 * (b[0] - Zero) + (b[1] - Zero);
            case 3: return 10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero);
            case 4: return 10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero);
            case 5: return 10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero);
            case 6: return 10 * (10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero)) + (b[5] - Zero);
            case 7: return 10 * (10 * (10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero)) + (b[5] - Zero)) + (b[6] - Zero);
            case 8: return 10 * (10 * (10 * (10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero)) + (b[5] - Zero)) + (b[6] - Zero)) + (b[7] - Zero);
            case 9: return 10 * (10 * (10 * (10 * (10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero)) + (b[5] - Zero)) + (b[6] - Zero)) + (b[7] - Zero)) + (b[8] - Zero);
            case 10: return 10 * (10 * (10 * (10 * (10 * (10 * (10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero)) + (b[4] - Zero)) + (b[5] - Zero)) + (b[6] - Zero)) + (b[7] - Zero)) + (b[8] - Zero)) + (b[9] - Zero);
            default: throw new ArgumentException("Int32 out of range count");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ParseD(byte[] b)
    {
        switch (b.Length)
        {
            case 1: return (b[0] - Zero);
            case 2: return 10 * (b[0] - Zero) + (b[1] - Zero);
            case 3: return 10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero);
            case 4: return 10 * (10 * (10 * (b[0] - Zero) + (b[1] - Zero)) + (b[2] - Zero)) + (b[3] - Zero);
        }

        int x = 0;
        for (int i = 0; i < b.Length; i++)
        {
            x *= 10;
            x += b[i] - Zero;
        }
        return x;
    }

    // '0' (U+30)を 8バイト並べたもの。
    const ulong LongZero = 0x30303030_30303030;
    const ushort ShortZero = 0x3030;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int ParseE(byte[] b)
    {
        byte* buf = stackalloc byte[10];

        fixed (byte* p = b)
        {
            *(ulong*)buf = *(ulong*)p - LongZero;
            *(ushort*)(buf + 8) = (ushort)(*(ushort*)(p + 8) - ShortZero);
        }

        int x = 0;
        for (int i = 0; i < b.Length; i++)
        {
            x = (x << 3) + (x << 1); // x * 10。C# はこの手の定数掛け算最適化してくれなかったと思うんで。
            x += buf[i];
        }
        return x;
    }
}