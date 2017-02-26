using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using System;

namespace ValueTypeGenerics
{
    [SimpleJob(RunStrategy.Throughput)]
    public class BenchmarkCode
    {
        static int[] _data;

        static BenchmarkCode()
        {
            var r = new Random();
            _data = new int[50];
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = r.Next();
            }
        }

        [Benchmark] public static void EmbeddedAdd() => Embedded.Sum(_data);
        [Benchmark] public static void InterfaceAdd() { IGroup g = new AddGroup(); InterfaceParameter.Sum(_data, g); }
        [Benchmark] public static void TypeClassAdd() => TypeClass.Sm<AddGroup>(_data);

        [Benchmark] public static void EmbeddedMul() => Embedded.Prod(_data);
        [Benchmark] public static void InterfaceMul() { IGroup g = new MulGroup(); InterfaceParameter.Sum(_data, g); }
        [Benchmark] public static void TypeClassMul() => TypeClass.Sm<MulGroup>(_data);
    }
}
