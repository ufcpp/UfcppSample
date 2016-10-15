#define ArrayImplementation

using System;
using System.Diagnostics;
using System.Text;

#if !ArrayImplementation
// こっちの実装だと、UTF-16の方がいちいちfixedが必要でちょっと性能悪い
using Utf8String = UtfString.Utf8.String;
using Utf16String = UtfString.Utf16.String;
#else
using Utf8String = UtfString.ArrayImplementation.Utf8.String;
using Utf16String = UtfString.ArrayImplementation.Utf16.String;
#endif

namespace ConsoleApplication1
{
    class Performance
    {
        public static void Check()
        {
            Console.WriteLine("絵文字あり");
            Check("ASCII: abcABC, Latin-1: ÀÁÂÃÄÅ, ελληνικά кириллица עִברִית ひらがな 한글 漢字, combining: áあ゙, emoji: 👩👩🏽👨‍👨‍👨‍👨‍👨‍👨‍👨👨🏻‍👩🏿‍👦🏽‍👦🏼");

            Console.WriteLine("絵文字なし");
            Check("ASCII: abcABC, Latin-1: ÀÁÂÃÄÅ, ελληνικά кириллица עִברִית ひらがな 한글 漢字, combining: áあ゙");

            Console.WriteLine("日本語");
            Check("寿限無、寿限無 五劫の擦り切れ 海砂利水魚の 水行末 雲来末 風来末 食う寝る処に住む処 藪ら柑子の藪柑子 パイポパイポパイポのシューリンガン シューリンガンのグーリンダイ グーリンダイのポンポコピーのポンポコナーの 長久命の長助");

            Console.WriteLine("絵文字のみ");
            Check("👩👩🏽👨‍👨‍👨‍👨‍👨‍👨‍👨👨🏻‍👩🏿‍👦🏽‍👦🏼🐀🐁🐂🐃🐄🐅🐆🐇🐈🐉🐊🐋🐌🐍🐎🐏🐐🐑🐒🐓🐔🐕🐖🐗🐘🐙🐚🐛🐜🐝🐞🐟🐠🐡🐢🐣🐤🐥🐦🐧🐨🐩🐪🐫🐬");

            Console.WriteLine("latin-1");
            Check("!\"#$%&'() 1234567890 AQWSEDRFTGYHUJIKOLP+@,./\\<>?_°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ");

            Console.WriteLine("ASCII");
            Check("!\"#$%&'() 1234567890 AQWSEDRFTGYHUJIKOLP+@,./\\<>?_");
        }

        static void Check(string s)
        {
            const int N = 100;
            // 計測用に長めの文字列にしたいので 2^10 倍につなぐ
            for (int i = 0; i < 10; i++)
            {
                s = s + s;
            }

            var utf8 = Encoding.UTF8.GetBytes(s);
#if !ArrayImplementation
            var utf16 = Encoding.Unicode.GetBytes(s);
#else
            var utf16 = Copy8To16(Encoding.Unicode.GetBytes(s));
#endif
            GC.Collect();

            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < N; i++)
                foreach (var c in new Utf8String(utf8))
                    ;
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
                foreach (var c in new Utf16String(utf16))
                    ;
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                var x = new Utf8String(utf8);
                foreach (var index in x.Indexes)
                {
                    var c = x[index];
                }
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            sw.Reset();

            sw.Start();
            for (int i = 0; i < N; i++)
            {
                var x = new Utf16String(utf16);
                foreach (var index in x.Indexes)
                {
                    var c = x[index];
                }
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        private static ushort[] Copy8To16(byte[] encodedBytes)
        {
            if ((encodedBytes.Length % 2) != 0) throw new ArgumentException();
            var output = new ushort[encodedBytes.Length / 2];
            Buffer.BlockCopy(encodedBytes, 0, output, 0, encodedBytes.Length);
            return output;
        }
    }
}
