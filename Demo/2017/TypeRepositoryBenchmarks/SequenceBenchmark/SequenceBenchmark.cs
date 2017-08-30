using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

/// <summary>
/// byte 列のsequence equalの速度。
///
///             Method |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
/// ------------------ |------------:|----------:|----------:|-------:|----------:|
///  LinqSequenceEqual | 2,328.58 ns | 7.8085 ns | 6.9220 ns | 0.1030 |     448 B |
///  SpanSequenceEqual |    59.82 ns | 0.4560 ns | 0.4266 ns |      - |       0 B |
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
}
