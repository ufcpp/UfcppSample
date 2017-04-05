using System;
using System.Runtime.CompilerServices;

namespace UnsafeRefReturns
{
    struct Color
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;
        public Color(byte a, byte r, byte g, byte b) => (A, R, G, B) = (a, r, g, b);
        public override string ToString() => (A, R, G, B).ToString();
    }

    namespace ColorSample
    {
        class Program
        {
            unsafe static void Main()
            {
                // 4バイトのデータ構造
                var color = new Color(255, 204, 204, 255);

                // 無理やり uint 扱いして、4バイト単位で読み書き
                var p = (uint*)&color;
                Console.WriteLine(p->ToString("X")); // FFCCCCFF

                *p = 0xFFFFA8F0;
                Console.WriteLine(color); // (240, 168, 255, 255)
            }

            // 一応 OK
            unsafe void NonGeneric(ref Color color)
            {
                fixed (Color* pc = &color)
                {
                    var p = (uint*)pc;
                    Console.WriteLine(p->ToString("X"));

                    *p = 0xFFFFA8F0;
                    Console.WriteLine(color);
                }
            }

#if false
        // これだとダメ
        // ジェネリック型のポインターは作れない
        unsafe void Generic<T>(ref T color)
        {
            fixed (T* pc = &color)
            {
                var p = (uint*)pc;
                Console.WriteLine(p->ToString("X"));

                *p = 0xFFFFA8F0;
                Console.WriteLine(color);
            }
        }
#endif

            // Unsafeクラスならできる
            // ジェネリック型のポインター化を無理やりできる
            unsafe void UseUnsafeClass<T>(ref T color)
            {
                ref uint p = ref Unsafe.AsRef<uint>(Unsafe.AsPointer(ref color));
                Console.WriteLine(p.ToString("X"));

                p = 0xFFFFA8F0;
                Console.WriteLine(color);
            }
        }
    }
}
