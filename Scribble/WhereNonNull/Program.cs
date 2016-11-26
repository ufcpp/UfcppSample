using BenchmarkDotNet.Running;

namespace WhereNonNull
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<OfTypeBenchmark>();
        }
    }
}
