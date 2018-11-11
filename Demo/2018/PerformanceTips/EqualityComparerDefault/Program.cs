using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

public struct Unit : IEquatable<Unit>
{
    public bool Equals(Unit other) => true;
    public override bool Equals(object obj) => obj is Unit;
    public override int GetHashCode() => 0;
}

public class EqualityComparerDefaultBenchmark
{
    [Benchmark]
    public bool IntEquals() => EqualityComparer<int>.Default.Equals(1, 2);

    [Benchmark]
    public bool StringEquals() => EqualityComparer<string>.Default.Equals("abc", "def");

    [Benchmark]
    public bool UserDefinedEquals() => EqualityComparer<Unit>.Default.Equals(default, default);
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<EqualityComparerDefaultBenchmark>(new MultipleRuntimesConfig());
    }
}
