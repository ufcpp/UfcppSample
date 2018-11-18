using BenchmarkDotNet.Running;

namespace DiscriminatedUnion
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<UnionBenchmark>();
        }
    }
}
