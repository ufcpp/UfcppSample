namespace Unsafe.CustomFixed
{
    using System.Runtime.CompilerServices;

    // ただの配列のラッパー
    readonly struct Array<T>
    {
        private readonly T[] _array;
        public Array(int length) => _array = new T[length];
        public ref T this[int index] => ref _array[index];
        public int Length => _array.Length;

        // このメソッドがあれば fixed ステートメントを使えるようになる
#if true
        public ref T GetPinnableReference() => ref _array[0];
#else
        // 本来の配列と挙動を併せたければこう書く
        public unsafe ref T GetPinnableReference()
        {
            var a = _array;
            if (a.Length == 0) return ref Unsafe.AsRef<T>(null);
            else return ref a[0];
        }
#endif
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new Array<int>(5);

            unsafe
            {
                // fixed (int* p = &a.GetPinnableReference()) に展開される。
                fixed (int* p = a)
                {
                    for (int i = 0; i < 5; i++)
                        p[i] = i;
                }
            }

            for (int i = 0; i < 5; i++)
                System.Console.WriteLine(a[i]);
        }
    }
}
