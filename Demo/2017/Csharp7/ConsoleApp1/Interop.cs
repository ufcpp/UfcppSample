using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static class Interop
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr malloc(int size);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free(IntPtr ptr);
    }
}
