using BenchmarkDotNet.Attributes;
using System.Linq;

namespace Enumeration
{
    [MemoryDiagnoser]
    public class EnumerationBenchmark
    {
        private ListLike<int> _data;

        [GlobalSetup]
        public void Setup()
        {
            var array = Enumerable.Range(0, 1000).ToArray();
            _data = new ListLike<int> { Array = array, Length = 500 };
        }

        [Benchmark] public int SumArray() => Sum.SumArray(_data);
        [Benchmark] public int SumSpan() => Sum.SumSpan(_data);
        [Benchmark] public int SumEnumerable() => Sum.SumEnumerable(_data);
        [Benchmark] public int SumFastEnumerable() => Sum.SumFastEnumerable(_data);
        [Benchmark] public int SumEnumeratorInterface() => Sum.SumEnumeratorInterface(_data.GetEnumerator());
        [Benchmark] public int SumFastEnumeratorInterface() => Sum.SumFastEnumeratorInterface(_data.GetFastEnumerator());
    }
}
