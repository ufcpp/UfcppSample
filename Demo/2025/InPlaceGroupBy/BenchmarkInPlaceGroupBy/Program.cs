using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using InPlaceGroupBy;
using System.Runtime.CompilerServices;

BenchmarkRunner.Run<InPlaceGroupByBenchmark>();

[MemoryDiagnoser]
public class InPlaceGroupByBenchmark
{
    public static (string key, int value)[] Data =
    [
        ("a", 1), ("ab", 2), ("abc", 3), ("a", 4), ("ab", 5),
        ("a", 6), ("a", 7), ("abc", 8), ("a", 9), ("a", 10),
        ("a", 11), ("ab", 12), ("abc", 13), ("b", 14), ("ab", 15),
        ("a", 16), ("a", 17), ("bc", 18), ("a", 19), ("b", 20),
        ("a", 21), ("ab", 22), ("ac", 23), ("a", 24), ("ab", 25),
        ("a", 26), ("d", 27), ("abc", 28), ("a", 29), ("ac", 30),
    ];

    [Benchmark]
    public void Linq()
    {
        var values = (stackalloc int[30]);
        var i = 0;

        foreach (var g in Data.GroupBy(x => x.key))
        {
            values[i++] = g.Sum(x => x.value);
        }
    }

    [InlineArray(32)]
    private struct Buffer<T>
    {
        private T _value;
    }

    [Benchmark]
    public void InPlaceSpan()
    {
        var buffer = new Buffer<(string key, int value)>();
        Span<(string key, int value)> span = buffer;
        Data.CopyTo(span); // needs copy

        var values = (stackalloc int[30]);
        var i = 0;

        foreach (var g in span.GroupBy((x, y) => x.key.AsSpan().CompareTo(y.key, StringComparison.Ordinal)))
        {
            values[i++] = g.Sum(x => x.value);
        }
    }
}

file static class Ex
{
    public static int Sum<T>(this Span<T> span, Func<T, int> selector)
    {
        var sum = 0;
        foreach (var item in span) sum += selector(item);
        return sum;
    }
}
