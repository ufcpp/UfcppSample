using BenchmarkDotNet.Running;
public class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<ArrayDowncastBenchmark>();
    }
}