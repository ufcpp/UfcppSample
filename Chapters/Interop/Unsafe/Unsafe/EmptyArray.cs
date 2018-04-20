namespace Unsafe.EmptyArray
{
    using System;
    using static System.Console;

    class Program
    {
        unsafe static void Main()
        {
            var array = new int[0]; // 空っぽ

            fixed (int* p = array) WriteLine((ulong)p); // 0

            try
            {
                // この書き方だと今度は例外になる。
                fixed (int* p = &array[0]) WriteLine((ulong)p);
            }
            catch (IndexOutOfRangeException)
            {
                WriteLine("IndexOutOfRangeException");
            }
        }
    }
}
