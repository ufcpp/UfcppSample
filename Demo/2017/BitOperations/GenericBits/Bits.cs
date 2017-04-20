using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GenericBits
{
    public unsafe struct Bits<T> : IEnumerable<bool>, IReadOnlyList<bool>
        where T : struct
    {
        private static readonly int NumBits = Unsafe.SizeOf<T>() * 8;

        private byte* _ptr;
        public Bits(ref T x) => _ptr = (byte*)Unsafe.AsPointer(ref x);

        const int Shift = 3;
        const int Mask = 0b111;

        public bool this[int index]
        {
            get
            {
                if (index >= NumBits) throw new IndexOutOfRangeException();
                return (_ptr[index >> Shift] & (1 << (index & Mask))) != 0;
            }
            set
            {
                if (index >= NumBits) throw new IndexOutOfRangeException();
                if (value) _ptr[index >> Shift] |= (byte)(1UL << (index & Mask));
                else _ptr[index >> Shift] &= (byte)~(1 << (index & Mask));
            }
        }

        public int Count => NumBits;

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<bool>
        {
            private int _i;
            private Bits<T> _bits;
            public Enumerator(Bits<T> bits) => (_bits, _i) = (bits, -1);

            public bool Current => _bits[_i];
            object IEnumerator.Current => Current;
            public void Dispose() { }
            public bool MoveNext() => ++_i < _bits.Count;
            public void Reset() => _i = -1;
        }
    }
}