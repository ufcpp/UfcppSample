namespace Span.VariousTypeOfMemory
{
    using System;
    using System.Runtime.InteropServices;

    class Program
    {
        static void Main()
        {
            // 配列
            Span<int> array = new int[8].AsSpan().Slice(2, 3);

            // 文字列
            ReadOnlySpan<char> str = "abcdefgh".AsReadOnlySpan().Slice(2, 3);

            // スタック領域
            Span<int> stack = stackalloc int[8];

            unsafe
            {
                // .NET 管理外メモリ
                var p = Marshal.AllocHGlobal(sizeof(int) * 8);
                Span<int> unmanaged = new Span<int>((int*)p, 8);

                // 他の言語との相互運用
                var q = malloc((IntPtr)(sizeof(int) * 8));
                Span<int> interop = new Span<int>((int*)q, 8);

                Marshal.FreeHGlobal(p);
                free(q);
            }
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr malloc(IntPtr size);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void free(IntPtr ptr);
    }
}
