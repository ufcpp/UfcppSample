using BenchmarkDotNet.Running;

class Program
{

    static void Main()
    {
        BenchmarkRunner.Run<EnumHasFlagBenchmark>(new MultipleRuntimesConfig());
    }
}
