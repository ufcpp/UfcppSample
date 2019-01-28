using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BoolOtherThan01
{
    class Program
    {
        static void Main()
        {
            // 0 → false
            // 1 → true
            // それ以外 → if (b) は通るんだけど、switch (b) { case true: } は通らない(C# 7.3 までは)変な値になる。
            for (byte i = 0; i < 3; i++)
            {
                Console.WriteLine($"value = {i}");
                Branch(Pointer(i));
                Branch(UnsafeAs(i));
                Branch(UnionStruct(i));
            }
        }

        /// <summary>
        /// false (0) の時は何も表示されない。
        /// true (1) の時は if(b) switch(b) の両方が表示される。
        /// 「それ以外の値」を作って渡すと、if(b) だけが表示される。
        /// </summary>
        static void Branch(bool b)
        {
            if (b) Console.WriteLine("    if(b)");
            switch (b) { case true: Console.WriteLine("    switch(b)"); break; }
        }

        private unsafe static bool Pointer(byte b) => *((bool*)&b);

        private static bool UnsafeAs(byte b) => Unsafe.As<byte, bool>(ref b);

        private static bool UnionStruct(byte b)
        {
            Union u = default;
            u.Byte = b;
            return u.Boolean;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Union
        {
            [FieldOffset(0)]
            public byte Byte;
            [FieldOffset(0)]
            public bool Boolean;
        }
    }
}
