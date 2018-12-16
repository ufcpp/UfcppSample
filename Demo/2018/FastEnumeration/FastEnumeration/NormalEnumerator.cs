using System;
using System.Collections;
using System.Collections.Generic;

namespace FastEnumeration
{
    /// <summary>
    /// <see cref="IEnumerator{T}"/> 実装。
    /// 単に配列を列挙。
    /// <see cref="Array.GetEnumerator"/> を呼ぶのでもいいんだけど、比較のために自作。
    /// パフォーマンス優先でチェックをさぼってる。MoveNext を int がオーバーフローするまで呼ぶと MoveNext が再び true を返すようになるけど、そんな使い方はしないという前提。
    /// </summary>
    class NormalEnumerator : IEnumerator<int>
    {
        private readonly int[] _data;
        public NormalEnumerator(int[] data) => _data = data;

        private int _i = -1;

        public int Current => _data[_i];
        public bool MoveNext() => ++_i < _data.Length;

        object IEnumerator.Current => Current;
        public void Dispose() { }
        public void Reset() => throw new NotImplementedException();
    }
}
