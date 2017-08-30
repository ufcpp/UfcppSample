using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

/// <summary>
/// byte 列のsequence equalの速度。
///
///             Method |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
/// ------------------ |------------:|----------:|----------:|-------:|----------:|
///  LinqSequenceEqual | 2,349.54 ns | 14.6437 ns | 13.6977 ns | 0.1640 |     704 B |
///  SpanSequenceEqual |    65.60 ns |  0.1678 ns |  0.1310 ns |      - |       0 B |
///    MySequenceEqual |   128.53 ns |  0.3044 ns |  0.2542 ns |      - |       0 B |
///
/// <see cref="IEnumerable{byte}"/>越しの操作がものすごい遅いのはしょうがないとして、
/// それを差し引いても<see cref="SpanExtensions.SequenceEqual(ReadOnlySpan{byte}, ReadOnlySpan{byte})"/>が爆速すぎてビビる。
/// </summary>
[MemoryDiagnoser]
public class SequenceBenchmark
{
    const int M = 5;
    const int N = 2000;
    byte[][] items;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random();

        items = new byte[M][];

        for (int i = 0; i < items.Length; i++)
        {
            var len = 1 + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5) + r.Next(0, 5);
            var item = new byte[len];
            for (int j = 0; j < item.Length; j++)
            {
                item[j] = (byte)r.Next(0, 256);
            }
            items[i] = item;
        }
    }

    [Benchmark]
    public void LinqSequenceEqual()
    {
        for (int i = 0; i < M; i++)
            for (int j = 0; j < M; j++)
            {
                var a = items[i];
                var b = items[j];

                var x = i == j;
                var y = Enumerable.SequenceEqual(a, b);

                if (x != y) throw new Exception();
            }
    }

    [Benchmark]
    public void SpanSequenceEqual()
    {
        for (int i = 0; i < M; i++)
            for (int j = 0; j < M; j++)
            {
                Span<byte> a = items[i];
                Span<byte> b = items[j];

                var x = i == j;
                var y = SpanExtensions.SequenceEqual(a, b);

                if (x != y) throw new Exception();
            }
    }

    [Benchmark]
    public void MySequenceEqual()
    {
        for (int i = 0; i < M; i++)
            for (int j = 0; j < M; j++)
            {
                var a = items[i];
                var b = items[j];

                var x = i == j;
                var y = MySequenceEqual(a, b);

                if (x != y) throw new Exception();
            }
    }

    private unsafe bool MySequenceEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;

        fixed (byte* pa = a)
        fixed (byte* pb = b)
            return MySequenceEqual(pa, pb, a.Length);
    }

    /// <summary>
    /// <see cref="Span{T}"/>が使えない環境向けに unsafe で自前実装。
    /// </summary>
    /// <remarks>
    /// <see cref="SpanExtensions.SequenceEqual(Span{byte}, ReadOnlySpan{byte})"/>は SIMD 命令とかまで使って相当最適化してあるので、正直勝つのは難しい。
    /// あくまで、System.Memory パッケージが使えない環境向けの救済策。
    /// </remarks>
    private unsafe static bool MySequenceEqual(byte* first, byte* second, int length)
    {
        if (length < 0) throw new ArgumentException();

        while (length >= 8)
        {
            if (*(ulong*)first != *(ulong*)second) return false;
            length -= 8;
            first += 8;
            second += 8;
        }
        while (length >= 0)
        {
            if (*first != *second) return false;
            --length;
            first += 1;
            second += 1;
        }
        return true;
    }
}
