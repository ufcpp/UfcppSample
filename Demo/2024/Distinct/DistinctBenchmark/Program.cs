using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<DistinctBenchmark>();

public class DistinctBenchmark
{
    [Params(10, 20, 50, 100, 200, 500, 1000)]
    public int N { get; set; }

    private int[] _data = null!;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random();
        _data = new int[1000];
        foreach (ref var x in _data.AsSpan()) x = r.Next(1, 1000);
    }

    [Benchmark(Baseline = true)]
    public void Linq()
    {
        foreach (var _ in _data.Take(N).Distinct()) ;
    }

    [Benchmark]
    public void Linear()
    {
        foreach (var _ in Distinct.Linear.Distinct(_data.Take(N), stackalloc int[N])) ;
    }

    [Benchmark]
    public void Binary()
    {
        foreach (var _ in Distinct.Binary.Distinct(_data.Take(N), stackalloc int[N])) ;
    }

    [Benchmark]
    public void Hash()
    {
        foreach (var _ in Distinct.Hash.Distinct(_data.Take(N), stackalloc int[N], x => x, x => x == 0)) ;
    }

    [Benchmark]
    public void Hash2N()
    {
        foreach (var _ in Distinct.Hash.Distinct(_data.Take(N), stackalloc int[2 * N], x => x, x => x == 0)) ;
    }
}