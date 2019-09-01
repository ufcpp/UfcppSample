namespace NullPointer.InvalidPointer
{
    using System;
    using System.Runtime.CompilerServices;

    class Program
    {
        unsafe static void Main()
        {
            NullPointer();
            InvalidPointer();
            UnsafeString();
        }

        private static unsafe void UnsafeString()
        {
            string s = null;

            byte* p = stackalloc byte[20];
            *(int*)(p + 8) = 3;
            *(long*)(p + 12) = 0x0043_0042_0041;
            Unsafe.As<string, IntPtr>(ref s) = (IntPtr)(void*)p;

            Console.WriteLine(s[0]); // A
            Console.WriteLine(s[1]); // B
            Console.WriteLine(s[2]); // C
        }

        unsafe static void NullPointer()
        {
            string s = "";
            Unsafe.As<string, IntPtr>(ref s) = (IntPtr)0;
            Console.WriteLine(s.Length);
        }

        unsafe static void InvalidPointer()
        {
            string s = "";
            Unsafe.As<string, IntPtr>(ref s) = (IntPtr)123456789;
            Console.WriteLine(s.Length);
        }
    }
}
