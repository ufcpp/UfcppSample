using BitFields;
using System;

namespace BitFieldsSample
{
    /// <summary>
    /// https://www.nuget.org/packages/BitFields/ の簡単なサンプル。
    ///
    /// このライブラリが提供してるのは以下のようなもの。
    ///
    /// - Bit{N} 型: N ビットまでしか受け付けない unsigned 整数型
    /// - BitNAnalyzer: N ビットまでしか受け付けない保証をするためのアナライザー
    /// - BitFieldsGenerator: <see cref="DoubleView"/>, <see cref="SingleView"/>, <see cref="Rgb555"/> みたいな型から、ビットフィールドを生成する Code-Fix 
    ///
    /// その利用例として以下のようなものを用意。
    ///
    /// - double/float (IEEE 754 形式浮動小数点数)の中身をビット操作
    /// - RGB555 形式(各色5ビットずつのRGB)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DoubleViewSample();
            SingleViewSample();
            Rgb555Sample();
        }

        private static void DoubleViewSample()
        {
            var v = new DoubleView();

            v.Sign = 1; // -
            v.Exponent = 3 + 1023; // 2^3
            v.Fraction = 0xE_0000_0000_0000; // 1.111b

            Console.WriteLine(v.Value.ToString("X"));
            Console.WriteLine(v.AsFloat()); // -15
        }

        private static void SingleViewSample()
        {
            var v = new SingleView();

            v.Sign = 1; // -
            v.Exponent = 3 + 127; // 2^3
            v.Fraction = 0x70_0000; // 1.111b

            Console.WriteLine(v.Value.ToString("X"));
            Console.WriteLine(v.AsFloat()); // -15
        }

        private static void Rgb555Sample()
        {
            var color = new Rgb555(10, 20, 30);
            Console.WriteLine(color.Value.ToString("X"));
            Console.WriteLine(color);
            (byte r, byte g, byte b) = color;
            Console.WriteLine((r, g, b));

#if false
            // #if を外すとコンパイルエラーになる

            var color1 = new Rgb555(10, 20, 32); // Bit5 なので 32 (6ビット)はダメ
            color1.R = 32;
#endif
        }

        static void BitNSample()
        {
            Bit1 x1a = 0;
            Bit1 x1b = 1;

            Bit4 x4a = 15; // 4ビットなので15までOK
            Bit63 x63a = 0x4000_0000_0000_0000;

#if false
            // #if を外すとコンパイルエラーになる

            Bit1 x1c = 2; // 2は2ビットなのでダメ
            Bit4 x4b = 16;
            Bit63 x63b = 0x8000_0000_0000_0000;
#endif
        }
    }
}