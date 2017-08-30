using BenchmarkDotNet.Running;

public class Program
{
    static void Main()
    {
#if tru
        var p = new ByteToLongBenchmark();
        p.Setup();
        p.PointerBenchmark();
        p.Pointer2Benchmark();
        p.SpanBenchmark();
        p.Test();
#else
        BenchmarkRunner.Run<ByteToLongBenchmark>();
#endif
    }
}
