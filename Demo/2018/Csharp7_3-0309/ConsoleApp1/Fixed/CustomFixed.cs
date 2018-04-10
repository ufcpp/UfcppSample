using System;

namespace ConsoleApp1.Fixed.CustomFixed
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Array<int>(5);

            unsafe
            {
                // 任意の型に対して fixed が使える。
                // fixed (int* p = &a.GetPinnableReference()) に展開される。
                // 主に Span<T> 用。
                // これまで、string に対しては特殊対応して fixed ステートメントを使えるようにしてたけど、今後は string に対しても GetPinnableReference を呼ぶようになるかも。
                fixed (int* p = a)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        p[i] = i;
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(a[i]);
            }
        }
    }

    readonly struct Array<T>
    {
        private readonly T[] _array;
        public Array(int length) => _array = new T[length];
        public ref T this[int index] => ref _array[index];
        public int Length => _array.Length;
        public ref T GetPinnableReference() => ref _array[0];
    }
}
