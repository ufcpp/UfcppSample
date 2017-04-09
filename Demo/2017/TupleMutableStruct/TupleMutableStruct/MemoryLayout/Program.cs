using BenchmarkDotNet.Running;

namespace TupleMutableStruct.MemoryLayout
{
    class Program
    {
        static void Main()
        {
            new VectorTest().XAndYShouldBeIdentical();
            BenchmarkRunner.Run<VectorPerformance>();
        }
    }
}
