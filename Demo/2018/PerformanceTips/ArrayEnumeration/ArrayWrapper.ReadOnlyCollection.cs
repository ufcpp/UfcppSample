using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ArrayEnumeration
{
    public partial struct ArrayWrapper<T>
    {
        // インデクサーも使いたいとき用、その1。
        // ReadOnlyCollection<T> を介してみる。
        // .NET Framework 2.0 時代のクラスなので、パフォーマンスへの考慮がまるでなく、相当遅い。
        public ReadOnlyCollection<T> AsReadOnlyCollection() => new ReadOnlyCollection<T>(Array);
    }

    public partial class ArrayEnumerationBenchmark
    {
        // ReadOnlyCollection<T> 列挙。
        // InterfaceEnumeration 以上に遅い。とにかく遅い。
        // ReadOnlyCollection<T> は内部的に IList<T> 越しに配列アクセスするので、それがほんとに遅い。
        [Benchmark]
        public int ReadOnlyCollectionEnumeration()
        {
            var sum = 0;
            foreach (var x in _array.AsReadOnlyCollection()) sum += x;
            return sum;
        }
    }
}
