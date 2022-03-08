using BenchmarkDotNet.Attributes;

namespace StringJoin;

[MemoryDiagnoser]
public class JoinBenchmark
{
    private static readonly byte[][] _data = new[]
    {
        new byte[] { 1, 3, 5, 7 },
        new byte[] { 2, 5, 9, 14, 20 },
        new byte[] { 192, 168, 0, 1 },
        new byte[] { 5, 6, 3 },
        new byte[] { 4, 4, 6 },
        new byte[] { 255, 255, 0, 0 },
    };

    [Benchmark]
    public string StringJoin() => string.Join(", ", _data.Select(sub => string.Join(".", sub)));

    [Benchmark]
    public string StringJoinX() => string.Join(", ", _data.Select(sub => string.Join(".", sub.Select(x => x.ToString("X")))));

    [Benchmark]
    public string JoinerJoin() => $"{Joiner.Join(", ", _data.Select(sub => Joiner.Join(".", sub)))}";

    [Benchmark]
    public string JoinerJoinX() => $"{Joiner.Join(", ", _data.Select(sub => Joiner.Join(".", sub))):X}";
}
