using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BitOperations
{
    // Bits<T> should be a ref-like type (https://github.com/dotnet/csharplang/blob/master/meetings/2016/LDM-2016-11-01.md)
    // Type parameter T should be blittabl (https://github.com/dotnet/csharplang/issues/187)
    public unsafe struct Bits<T, TOperator> : IBits
        where T : struct
        where TOperator : SBitOperator<T>
    {
        // This should use ByReference<T> (https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/ByReference.cs) if possible
        void* _ptr;
        public Bits(ref T x) => _ptr = Unsafe.AsPointer(ref x);

        /// <summary>
        /// get/set a <paramref name="index"/>-th bit。
        /// </summary>
        /// <remarks>
        /// It depends on <see cref="TOperator"/>'s implementaion whether order of bits is ascending or descending。
        /// </remarks>
        public bool this[int index]
        {
            get => default(TOperator).GetBit(ref Unsafe.AsRef<T>(_ptr), index);
            set => default(TOperator).SetBit(ref Unsafe.AsRef<T>(_ptr), index, value);
        }

        /// <summary>
        /// bit counts.
        /// </summary>
        public int Count => default(TOperator).Size;

        Enumerator GetEnumerator() => new Enumerator(Unsafe.AsRef<T>(_ptr));
        IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        bool IReadOnlyList<bool>.this[int index] => this[index];

        public struct Enumerator : IEnumerator<bool>
        {
            int _i;
            T _x;
            public Enumerator(T x) => (_x, _i) = (x, 0);

            public bool Current => default(TOperator).GetBit(ref _x, 0);
            object IEnumerator.Current => Current;
            void IDisposable.Dispose() { }
            public bool MoveNext()
            {
                if (_i == 0)
                {
                    ++_i;
                    return true;
                }
                if (_i >= default(TOperator).Size) return false;
                ++_i;
                _x = default(TOperator).RightShift(_x);
                return true;
            }

            public void Reset() => throw new NotImplementedException();
        }
    }
}
