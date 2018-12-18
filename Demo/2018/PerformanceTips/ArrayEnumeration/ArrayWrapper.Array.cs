using BenchmarkDotNet.Attributes;

namespace ArrayEnumeration
{
    /// <summary>
    /// <see cref="System.Collections.Immutable.ImmutableArray{T}"/> みたいに、
    /// 内部的に配列を1個持ってるだけのコレクションから要素を列挙することを考える。
    /// </summary>
    public partial struct ArrayWrapper<T>
    {
        // 比較のために生列挙をしたいので public (本来は不要)
        public readonly T[] Array;
        public ArrayWrapper(T[] array) => Array = array;
    }

    public partial class ArrayEnumerationBenchmark
    {
        public ArrayWrapper<int> _array;

        [GlobalSetup]
        public void Setup()
        {
            var array = new int[1024];
            for (int i = 0; i < array.Length; i++) array[i] = i;
            _array = new ArrayWrapper<int>(array);
        }

        // 比較のための生列挙。
        [Benchmark(Baseline = true)]
        public int RawEnumeration()
        {
            var sum = 0;
            foreach (var x in _array.Array) sum += x;
            return sum;
        }
    }
}
