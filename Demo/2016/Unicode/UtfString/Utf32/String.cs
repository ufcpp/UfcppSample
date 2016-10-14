// どの String 実装でもほぼ共通のコード。
// 基本的にファイル コピー。変更点は以下の2つだけ
// - 名前空間
// - コンストラクター引数と、ArrayAccessor を作る部分
//
// インターフェイスを切れば1個の実装で済むけども、以下の理由でファイル コピーにしてる。
// - パフォーマンス最優先で、static メソッドを直参照
//   - ヒープ確保しないように
// - 違うエンコーディングの Index を渡せてしまうと動作保証外なので、別の型にしたい

using System;
using System.Collections;
using System.Collections.Generic;

namespace UtfString.Utf32
{
    public struct String : IEnumerable<CodePoint>, IString<Index, StringEnumerator, IndexEnumerable, IndexEnumerable>
    {
        private readonly ArrayAccessor _buffer;

        public String(byte[] encodedBytes)
        {
            _buffer = new ArrayAccessor(encodedBytes);
        }

        public StringEnumerator GetEnumerator() => new StringEnumerator(_buffer);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        IEnumerator<CodePoint> IEnumerable<CodePoint>.GetEnumerator() => GetEnumerator();

        public IndexEnumerable Indexes => new IndexEnumerable(_buffer);
        public CodePoint this[Index index] => Decoder.Decode(_buffer, index);

        public int Length => Decoder.GetLength(_buffer);
    }

    public struct Index
    {
        internal int index;
        internal byte count;
    }

    public struct StringEnumerator : IEnumerator<CodePoint>
    {
        private readonly ArrayAccessor _buffer;
        private Index _index;

        public StringEnumerator(ArrayAccessor buffer)
        {
            _buffer = buffer;
            _index = default(Index);
            Current = default(CodePoint);
        }

        public CodePoint Current { get; private set; }

        public bool MoveNext()
        {
            _index.index += _index.count;
            var next = Decoder.TryDecode(_buffer, _index.index);
            if (next.count == Constants.InvalidCount) return false;
            _index.count = next.count;
            Current = next.cp;
            return true;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }
    }

    public struct IndexEnumerable : IEnumerator<Index>, IEnumerable<Index>, IIndexEnumerable<Index, IndexEnumerable>
    {
        private readonly ArrayAccessor _buffer;
        private Index _i;

        public IndexEnumerable(ArrayAccessor buffer)
        {
            _buffer = buffer;
            _i = default(Index);
        }

        public Index Current => _i;
        public bool MoveNext()
        {
            _i.index += _i.count;
            _i.count = Decoder.TyrGetCount(_buffer, _i.index);
            return _i.count != Constants.InvalidCount;
        }

        object IEnumerator.Current => Current;
        void IDisposable.Dispose() { }
        void IEnumerator.Reset() { throw new NotSupportedException(); }

        public IndexEnumerable GetEnumerator() => new IndexEnumerable(_buffer);
        IEnumerator<Index> IEnumerable<Index>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
