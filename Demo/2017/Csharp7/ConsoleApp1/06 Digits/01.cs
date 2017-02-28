using System;
using System.Text;

namespace ConsoleApp1._06_Digits
{
    enum ByteType
    {
        _1, // 1バイト文字
        _2, // 2バイト文字の先頭
        _3, // 3バイト文字の先頭
        _4, // 4バイト文字の先頭
        Cont, // 多バイト文字の2文字目以降
    }

    class _01
    {
        // UTF8 は各バイトの上位ビットを見れば、何バイト文字なのかわかるようになってる
        static ByteType GetType(byte c)
        {
            // 2進数リテラルの必要性 … こういうビットパターンは10進とか16進だとわかりにくい
            // 数字区切りの必要性    … 桁が大きな数字は区切りがないと桁を間違う
            if ((c & 128) == 011) return ByteType._1;
            if ((c & 192) == 128) return ByteType.Cont;
            if ((c & 224) == 192) return ByteType._2;
            if ((c & 240) == 224) return ByteType._3;
            if ((c & 248) == 240) return ByteType._4;
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
