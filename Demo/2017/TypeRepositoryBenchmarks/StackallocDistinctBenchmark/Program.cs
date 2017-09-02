using BenchmarkDotNet.Running;
using System;
using System.Linq;

public class Program
{
    static void Main()
    {
        var x = new DistinctBenchmark();
        x.Setup();

        if (!x.Linq0().SequenceEqual(x.Stack0())) throw new Exception();
        if (!x.Linq1().SequenceEqual(x.Stack1())) throw new Exception();
        if (!x.Linq2().SequenceEqual(x.Stack2())) throw new Exception();
        if (!x.Linq3().SequenceEqual(x.Stack3())) throw new Exception();
        if (!x.Linq0().SequenceEqual(x.Array0())) throw new Exception();
        if (!x.Linq1().SequenceEqual(x.Array1())) throw new Exception();
        if (!x.Linq2().SequenceEqual(x.Array2())) throw new Exception();
        if (!x.Linq3().SequenceEqual(x.Array3())) throw new Exception();

        BenchmarkRunner.Run<DistinctBenchmark>();
    }
}