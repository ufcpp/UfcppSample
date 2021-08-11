using System;
using System.Runtime.InteropServices;

namespace GenericMath
{
    class FixedArraySample
    {
        public static void M()
        {
            // 「generic math」以外でまず真っ先に「固定長配列」をやりたかったんだけど…

            var array = new FiexedArray2<string>();

            // これが型引数の推論効かないの残念…
            //var s = array.AsSpan();

            // これなら書けるけどあんまり書きたくない。
            // 一応、型引数の推論を賢くしたいという話はずっとある。(ずっとある…)
            var s = array.AsSpan<FiexedArray2<string>, string>();

            s[0] = "abc";
            s[1] = "xyz";

            Console.WriteLine(array);
        }
    }

    interface IArray<TArray, TElement>
        where TArray : IArray<TArray, TElement>
    {
        public static abstract int Length { get; }

        // 構造体は return ref this できないので、static に return ref parameter できないか試す。
        public static abstract ref TElement GetFirstElementRef(ref TArray array);
    }

    static class ArrayExtensions
    {
        // AsSpan も ref this 拡張メソッドとして実装。
        public static Span<TElement> AsSpan<TArray, TElement>(ref this TArray array)
            where TArray : struct, IArray<TArray, TElement>
            => MemoryMarshal.CreateSpan(ref TArray.GetFirstElementRef(ref array), TArray.Length);
    }

    // これの要素数違いは構造が機械的なのでテンプレート生成可能。
    public struct FiexedArray2<T> : IArray<FiexedArray2<T>, T>
    {
        private T _0;
        private T _1;

        public static int Length => 2;
        public static ref T GetFirstElementRef(ref FiexedArray2<T> array) => ref array._0;

        public override string ToString() => $"[ {_0}, {_1} ]";
    }
}
