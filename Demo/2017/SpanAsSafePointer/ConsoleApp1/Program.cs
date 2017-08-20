using BenchmarkDotNet.Running;

class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<SaveLoadBenchmark>();
    }
}
