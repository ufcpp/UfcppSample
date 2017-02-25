using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;

namespace HeapAllocation
{
    /// <summary>
    /// <see cref="Allocation"/>に対するベンチマーク。
    /// </summary>
    [SimpleJob(RunStrategy.Throughput)]
    public class AllocationBenchmark
    {
        const int Loops = 10000;

        [Benchmark]
        public static (int x, int y) Struct() => Allocation.Struct(Loops);

        [Benchmark]
        public static (int x, int y) GarbageCollection() => Allocation.GarbageCollection(Loops);

        [Benchmark]
        public unsafe static (int x, int y) Malloc0() => Allocation.Malloc0(Loops);

        [Benchmark]
        public static (int x, int y) Malloc() => Allocation.Malloc(Loops);

        [Benchmark]
        public static (int x, int y) LockPoolPointer() => Allocation.LockPoolPointer(Loops);

        [Benchmark]
        public static (int x, int y) CasPoolPointer() => Allocation.CasPoolPointer(Loops);

        [Benchmark]
        public static (int x, int y) LocalPoolPointer() => Allocation.LocalPoolPointer(Loops);

    }
}
