using BenchmarkDotNet.Attributes;
using System;

public class DefaultIsNullBenchmark
{
    public bool IsValueType1<T>() => typeof(T).IsValueType;
    public bool IsValueType2<T>() => default(T) != null;

    [Benchmark]
    public bool TypeIsValueType()
        => IsValueType1<int>() && IsValueType1<DateTime>() && !IsValueType1<string>();

    [Benchmark]
    public bool DefaultIsNull()
        => IsValueType2<int>() && IsValueType2<DateTime>() && !IsValueType2<string>();
}
