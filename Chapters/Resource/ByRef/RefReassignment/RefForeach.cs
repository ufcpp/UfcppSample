namespace ByRef.RefReassignment.RefForeach
{
    using System;

    class Program
    {
        static void Main()
        {
            var array = new int[10];
            foreach (ref var x in array.AsRef())
            {
                // ちゃんとこれで、配列の各要素を書き換えられる。
                x = 1;
            }

            foreach (var x in array)
            {
                // 全要素 1 になってる。
                Console.WriteLine(x);
            }
        }
    }

    // 標準で ref 戻り値になっている Enumerable はないので自作。
    struct RefArrayEnumerable<T>
    {
        T[] _array;
        public RefArrayEnumerable(T[] array) => _array = array;
        public RefArrayEnumerator<T> GetEnumerator() => new RefArrayEnumerator<T>(_array);
    }

    struct RefArrayEnumerator<T>
    {
        int _index;
        T[] _array;
        public RefArrayEnumerator(T[] array) => (_index, _array) = (-1, array);
        // Current が ref 戻り値になっているのがポイント。
        public ref T Current => ref _array[_index];
        public bool MoveNext() => ++_index < _array.Length;
    }

    static class RefExtensions
    {
        public static RefArrayEnumerable<T> AsRef<T>(this T[] array) => new RefArrayEnumerable<T>(array);
    }
}
