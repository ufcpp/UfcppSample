using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Globalization;
using System.Runtime.CompilerServices;

BenchmarkRunner.Run<StringInterpolationBenchmark>();

[MemoryDiagnoser]
public class StringInterpolationBenchmark
{
    private const int N = 10;
    private const int InitialBufferSize = 32;

    [Benchmark]
    public void OldStyle()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        // 昔の $"{a}.{b}.{c}.{d}" は string.Format に展開されてた。
        // C# 10.0 以降でも、DefaultInterpolatedStringHandler がない(TargetFramework が .NET 5 以下とか)だとこれになる。
        static string m(int a, int b, int c, int d) => string.Format("{0}.{1}.{2}.{3}", a, b, c, d);
    }

    [Benchmark]
    public void Improved()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        static string m(int a, int b, int c, int d) => $"{a}.{b}.{c}.{d}";
    }

    private static readonly CultureInfo _currentCulture = CultureInfo.CurrentCulture;
    private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

    [Benchmark]
    public void InvariantCulture()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        static string m(int a, int b, int c, int d) => string.Create(_invariantCulture, $"{a}.{b}.{c}.{d}");
    }

    [Benchmark]
    public void InitialBuffer()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        static string m(int a, int b, int c, int d) => string.Create(_currentCulture, stackalloc char[InitialBufferSize], $"{a}.{b}.{c}.{d}");
    }

    [Benchmark]
    public void InitialBufferInvariantCulture()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        static string m(int a, int b, int c, int d) => string.Create(_invariantCulture, stackalloc char[InitialBufferSize], $"{a}.{b}.{c}.{d}");
    }

    [Benchmark]
    public void InitialBufferSkipLocalsInitInvariantCulture()
    {
        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d);

        [SkipLocalsInit]
        static string m(int a, int b, int c, int d) => string.Create(_invariantCulture, stackalloc char[InitialBufferSize], $"{a}.{b}.{c}.{d}");
    }

    [Benchmark]
    [SkipLocalsInit]
    public void InitialSingleBufferInvariantCulture()
    {
        Span<char> buffer = stackalloc char[InitialBufferSize];

        for (int a = 0; a < N; a++)
            for (int b = 0; b < N; b++)
                for (int c = 0; c < N; c++)
                    for (int d = 0; d < N; d++)
                        m(a, b, c, d, buffer);

        static string m(int a, int b, int c, int d, Span<char> buffer) => string.Create(_invariantCulture, buffer, $"{a}.{b}.{c}.{d}");
    }
}