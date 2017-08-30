using BenchmarkDotNet.Running;

class Program
{
    static void Main(string[] args)
    {
#if tru
        var x = new SequenceBenchmark();
        x.Setup();
        x.LinqSequenceEqual();
        x.SpanSequenceEqual();
#else
        BenchmarkRunner.Run<SequenceBenchmark>();
#endif
    }
}
