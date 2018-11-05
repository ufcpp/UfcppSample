using BenchmarkDotNet.Running;

namespace Enumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<EnumerationBenchmark>();
        }
    }
}
