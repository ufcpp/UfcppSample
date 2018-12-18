using BenchmarkDotNet.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ArrayEnumeration
{
    public partial struct ArrayWrapper<T> : IEnumerable<T>
    {
        // 具象型の Enumerator を返しているだけでは IEnumerable<T> になれないので、
        // 別途明示的実装で IEnumerator<T> を返す GetEnumerator を用意。
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => new EnumeratorObject(Array);
        IEnumerator IEnumerable.GetEnumerator() => new EnumeratorObject(Array);

        // 構造体の Enumerator と中身は全く同じで、ただクラスになってるだけ。
        // 構造体をインターフェイス越しに返すとかえって遅くなるので、こんなクラスが別途必要に…
        public class EnumeratorObject : IEnumerator<T>
        {
            private readonly T[] _array;
            private int _i;
            internal EnumeratorObject(T[] array) => (_array, _i) = (array, -1);

            public T Current => _array[_i];
            public bool MoveNext() => ((uint)++_i) < (uint)_array.Length;

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotImplementedException();
        }
    }

    public partial class ArrayEnumerationBenchmark
    {
        // インターフェイス越し列挙になるように、IEnumerable<T> にキャストして使ってる。
        // びっくりするくらい遅い。
        // StructEnumeration とかに比べて10倍遅い。
        [Benchmark]
        public int InterfaceEnumeration()
        {
            var sum = 0;
            foreach (var x in (IEnumerable<int>)_array) sum += x;
            return sum;
        }
    }
}
