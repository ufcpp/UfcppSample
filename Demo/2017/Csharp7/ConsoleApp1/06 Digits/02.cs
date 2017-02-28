using System;
using System.Text;

namespace ConsoleApp1._06_Digits
{
    class _02
    {
        static ByteType GetType(byte c)
        {
            // 2進数リテラル … 0b から始める数値リテラルは2進数になる
            // 数字区切り … 数字と数字の間に _ を挟める
            if ((c & 0b1000_0000) == 0b0000_0000) return ByteType._1;
            if ((c & 0b1100_0000) == 0b1000_0000) return ByteType.Cont;
            if ((c & 0b1110_0000) == 0b1100_0000) return ByteType._2;
            if ((c & 0b1111_0000) == 0b1110_0000) return ByteType._3;
            if ((c & 0b1111_1000) == 0b1111_0000) return ByteType._4;
            throw new ArgumentOutOfRangeException();
        }

        // 実際、Utf8String (https://github.com/dotnet/corefxlab/tree/master/src/System.Text.Primitives)が最初の利用者かも
        // 2進数リテラルが入るまでの間は、
        const int b1000_0000 = 0x80;
        // みたいな定数が並んでた

        public static void Run()
        {
            var utf8 = Encoding.UTF8.GetBytes("aαあ亜👤🐈");

            foreach (var c in utf8)
            {
                var t = GetType(c);
                Console.WriteLine((c, t));
            }
        }
    }
}
