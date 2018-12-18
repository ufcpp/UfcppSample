using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace ArrayEnumeration
{
    public partial struct ArrayWrapper<T>
    {
        // インデクサーも使いたいとき用、その3。
        // ImmutableArray<T> を介してみる。
        // 実装を見てる感じ、少なくとも struct Enumerator と同じスピードは出そうなものなのに…
        // なんかなぜか遅い…
        public ImmutableArray<T> AsImmurable() => ImmutableArray.Create(Array);
    }

    public partial class ArrayEnumerationBenchmark
    {
        // AsImmurable<T> 列挙。
        // なんで遅いのかわからない…
        // (といっても、Interface 列挙や ReadOnlyCollection<T> 列挙よりは速い。)
        [Benchmark]
        public int ImmutableArrayEnumeration()
        {
            var sum = 0;
            foreach (var x in _array.AsImmurable()) sum += x;
            return sum;
        }
    }
}
