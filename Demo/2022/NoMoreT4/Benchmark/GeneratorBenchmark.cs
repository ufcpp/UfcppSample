using BenchmarkDotNet.Attributes;
using ClassLibrary1;

namespace Benchmark;

public class GeneratorBenchmark
{
    private static string Gen<T>()
        where T : IGenerator
        => T.Create(typeof(Sample)).TransformText();

    [Benchmark]
    public string T4() => Gen<T4Generator>();

    [Benchmark]
    public string Interpolation() => Gen<InterpolationGenerator>();
}
