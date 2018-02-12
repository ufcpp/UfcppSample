using System;
using System.Text;
using BenchmarkDotNet.Running;

class Program
{
    static readonly Encoding utf8 = Encoding.UTF8;

    static void Main()
    {
#if true
        var benchmark = new SubstringBenchmark();

        var x = benchmark.StandardString();
        var y = benchmark.AbstractString();
        var z = benchmark.SpanChar();
        for (int i = 0; i < 128; i++)
        {
            if (x[i] != y[i]) throw new Exception();
            if (x[i] != z[i]) throw new Exception();
        }
#endif
        BenchmarkRunner.Run<SubstringBenchmark>();
    }
}
