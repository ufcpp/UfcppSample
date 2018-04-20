namespace Unsafe.StringInternal
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Console;

    unsafe class Program
    {
        static ulong AsUnmanaged<T>(T r) where T : class => (ulong)Unsafe.As<T, IntPtr>(ref r);
        static ulong AsUnmanaged<T>(ref T r) => (ulong)Unsafe.AsPointer(ref r);

        static void Main()
        {
            var s = "abcde";

            WriteLine("s: " + AsUnmanaged(s));

            fixed(char* p = s)
            {
                WriteLine("&s[0]: " + (ulong)p);
            }

            WriteLine("&s[0] - s: " + RuntimeHelpers.OffsetToStringData);
        }
    }
}
