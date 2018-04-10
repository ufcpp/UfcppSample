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
