using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BitOperations
{
    // C# 7.2 (?)で ref-like type 機能(ref に準ずる構造体は ref 同様、フィールドにできないとかの制限を受けるべき)が入るなら、この型は ref-like であるべき
    // C# 7.2 で機能が入るなら、型引数 T は blittable (ブロック転送可能、manged pointer を含んではいけない、ポインター化できるという型制約)であるべき
    public unsafe struct Bits<T, TOperator> : IBits
        where T : struct
        where TOperator : SBitOperator<T>
    {
        // とりあえず今はポインターを使って実装
        // ↓が正式採用されれば、safe コンテキストで実装可能
        // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/ByReference.cs
        void* _ptr;
        public Bits(ref T x) => _ptr = Unsafe.AsPointer(ref x);

        /// <summary>
        /// <paramref name="index"/>ビット目を取得。
        /// </summary>
        /// <remarks>
        /// 下位から列挙するか上から列挙するかは<see cref="TOperator"/>の実装次第。
        /// </remarks>
        /// <param name="index">インデックス。</param>
        /// <returns>ビットが1ならtrue、0ならfalse。</returns>
        public bool this[int index]
        {
            get => default(TOperator).GetBit(ref Unsafe.AsRef<T>(_ptr), index);
            set => default(TOperator).SetBit(ref Unsafe.AsRef<T>(_ptr), index, value);
        }

        /// <summary>
        /// ビット長。
        /// </summary>
        public int Count => default(TOperator).Size;

        Enumerator GetEnumerator() => new Enumerator(Unsafe.AsRef<T>(_ptr));
        IEnumerator<bool> IEnumerable<bool>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        bool IReadOnlyList<bool>.this[int index] => this[index];

        /// <summary>
        /// 各ビットを順に列挙する enumerator。
        /// </summary>
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
