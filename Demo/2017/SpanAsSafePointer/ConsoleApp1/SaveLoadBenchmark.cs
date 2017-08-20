using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

/// <summary>
/// 実行結果一例:
///    Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
/// --------- |----------:|----------:|----------:|-------:|----------:|
///      Safe | 209.03 ns | 0.3908 ns | 0.3464 ns | 0.1142 |     480 B |
///   Unsafe1 | 117.81 ns | 0.4740 ns | 0.4202 ns | 0.0379 |     160 B |
///   Unsafe2 |  29.49 ns | 0.0974 ns | 0.0911 ns | 0.0381 |     160 B |
///  SpanBase |  40.54 ns | 0.1086 ns | 0.1016 ns | 0.0381 |     160 B |
/// </summary>
[MemoryDiagnoser]
public class SaveLoadBenchmark
{
    private const int N = 3;
    private Point[] _source;
    private Point[] _destination;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new Point[N];
        _destination = new Point[N];

        var r = new Random();
        for (int i = 0; i < _source.Length; i++)
        {
            _source[i] = new Point(r.NextDouble(), r.NextDouble(), r.NextDouble());
        }
    }

    [Benchmark]
    public void Safe() => default(ConsoleApp1.Safe.Copier).Copy(_source, _destination);

    [Benchmark]
    public void Unsafe1() => default(ConsoleApp1.Unsafe1.Copier).Copy(_source, _destination);

    [Benchmark]
    public void Unsafe2() => default(ConsoleApp1.Unsafe2.Copier).Copy(_source, _destination);

    [Benchmark]
    public void SpanBase() => default(ConsoleApp1.SpanBase.Copier).Copy(_source, _destination);
}
