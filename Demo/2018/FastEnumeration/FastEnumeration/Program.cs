using BenchmarkDotNet.Running;

namespace FastEnumeration
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<ForeachBenchmark>();
        }
    }
}
