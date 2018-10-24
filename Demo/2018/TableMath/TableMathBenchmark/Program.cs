using BenchmarkDotNet.Running;

namespace TableMathBenchmark
{
    static class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<TableBenchmark>();
        }
    }
}
