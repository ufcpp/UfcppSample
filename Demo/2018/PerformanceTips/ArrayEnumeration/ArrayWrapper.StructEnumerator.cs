using BenchmarkDotNet.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ArrayEnumeration
{
    public partial struct ArrayWrapper<T>
    {
        // 「仮想呼び出しは遅い」ということがわかっているわけで、
        // こんな感じで具象型を返す GetEnumerator を作った方が高速。
        // 構造体にした方が最適化が効く。
        public Enumerator GetEnumerator() => new Enumerator(Array);

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private int _i;
            internal Enumerator(T[] array) => (_array, _i) = (array, -1);

            public T Current => _array[_i];
            public bool MoveNext() => ((uint)++_i) < (uint)_array.Length;
            // ↑ uint がオーバーフローするまで MoveNext を空呼びされると結果が狂うけど、
            // だいぶ速度が変わるので、めったにそんなことやる人いない空呼びのことを気にするのは止めておく。

            object IEnumerator.Current => Current;
            public void Dispose() { }
            public void Reset() => throw new NotImplementedException();
        }
    }

    public partial class ArrayEnumerationBenchmark
    {
        // 構造体の Enumerator 越しの列挙
        // 構造体で返してるとほんとにきっちり最適化が効くみたいで、
        // ほぼ配列生列挙と同じ速度が出る。
        [Benchmark]
        public int StructEnumeration()
        {
            var sum = 0;
            foreach (var x in _array) sum += x;
            return sum;
        }
    }
}
