using BenchmarkDotNet.Attributes;
using System;

namespace ArrayEnumeration
{
    public partial struct ArrayWrapper<T>
    {
        // インデクサーも使いたいとき用、その2。
        // Span<T> を介してみる。
        // パフォーマンスに焦点が当たってた .NET Core 2.1 世代の型だけあって、かなり速い。
        public ReadOnlySpan<T> AsSpan() => Array;
    }

    public partial class ArrayEnumerationBenchmark
    {
        // Span<T> 列挙
        // こいつも配列生列挙とほぼ同じ性能。速い。
        [Benchmark]
        public int SpanEnumeration()
        {
            var sum = 0;
            foreach (var x in _array.AsSpan()) sum += x;
            return sum;
        }
    }
}
