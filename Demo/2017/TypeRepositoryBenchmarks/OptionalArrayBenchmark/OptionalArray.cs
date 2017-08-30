#define USE_BITARRAY64

using System;
using System.Collections.Generic;
using System.Collections;

#if USE_BITARRAY64
using BitArray = BitArray64;
#else
using BitArray = System.Collections.BitArray;
#endif

struct OptionalArray<T> : IReadOnlyList<Optional<T>>
{
    private T[] _array;
    private BitArray _hasValue;

    public OptionalArray(int length)
    {
        _array = new T[length];
#if USE_BITARRAY64
        _hasValue = new BitArray();
#else
        _hasValue = new BitArray(length);
#endif
    }

    public Optional<T> this[int index]
    {
        get => new Optional<T>(_array[index], _hasValue[index]);
        set
        {
            _array[index] = value.GetValueOrDefault();
            _hasValue[index] = value.HasValue;
        }
    }

    public int Length => _array.Length;
    int IReadOnlyCollection<Optional<T>>.Count => _array.Length;

    public OptionalArrayEnumerator GetEnumerator() => new OptionalArrayEnumerator(_array, _hasValue);
    IEnumerator<Optional<T>> IEnumerable<Optional<T>>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct OptionalArrayEnumerator : IEnumerator<Optional<T>>
    {
        private T[] _array;
        private BitArray _hasValue;
        private int _index;

        public OptionalArrayEnumerator(T[] array, BitArray hasValue) : this()
        {
            _array = array;
            _hasValue = hasValue;
            _index = -1;
        }

        public Optional<T> Current => new Optional<T>(_array[_index], _hasValue[_index]);
        public bool MoveNext()
        {
            // 速度 >> 安全性 な実装
            _index++;
            return _index < _array.Length;
        }

        object IEnumerator.Current => Current;
        public void Dispose() { }
        public void Reset() => throw new NotImplementedException();
    }
}
