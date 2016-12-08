using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace FixedPacker
{
    public struct Unpacker<T>
        where T : struct
    {
        private ReadOnlySpan<byte> _data;

        public Unpacker(ReadOnlySpan<byte> data) { _data = data; }

        public IEnumerator<T> GetEnumerator()
        {
            var size = Size;
            for (int i = 8; i < Length * Size; i += size)
            {
                yield return _data.Slice(i, size).Cast<byte, T>()[0];
            }
        }

        public int Length => (int)_data.Cast<byte, long>()[0];
        private int Size => Unsafe.SizeOf<T>();

        public Utf8String GetString(int index)
        {
            index += 8;
            index += Length * Size;
            var len = _data.Slice(index).Cast<byte, int>()[0];
            return new Utf8String(_data.Slice(index + 4, len));
        }
    }
}
