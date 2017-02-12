using System;
using System.Text;

namespace ConsoleApp1._06_Digits
{
    class _01
    {
        enum ByteType
        {
            _1, // 1バイト文字
            _2, // 2バイト文字の先頭
            _3, // 3バイト文字の先頭
            _4, // 4バイト文字の先頭
            Cont, // 多バイト文字の2文字目以降
        }

        // UTF8 は各バイトの上位ビットを見れば、何バイト文字なのかわかるようになってる
        static ByteType GetType(byte c)
        {
            // 2進数リテラル … こういうビットパターンは10進とか16進だとわかりにくい
            // 数字区切り … 桁が大きな数字は区切りがないと桁を間違う
            if ((c & 0b1000_0000) == 0b0000_0000) return ByteType._1;
            if ((c & 0b1100_0000) == 0b1000_0000) return ByteType.Cont;
            if ((c & 0b1110_0000) == 0b1100_0000) return ByteType._2;
            if ((c & 0b1111_0000) == 0b1110_0000) return ByteType._3;
            if ((c & 0b1111_1000) == 0b1111_0000) return ByteType._4;
            throw new ArgumentOutOfRangeException();
        }

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
