#define ArrayImplementation

using System;
using System.Diagnostics;
using System.Text;
using System.Linq;

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
            var s = "a";
            var utf8 = Encoding.UTF8.GetBytes(s);
#if !ArrayImplementation
            var utf16 = Encoding.Unicode.GetBytes(s);
#else
            var utf16 = Copy8To16(Encoding.Unicode.GetBytes(s));
#endif

            // JIT の時間カウントしないように最初に1度アクセス
            var s1 = new Utf8String(utf8);
            var i1 = s1.Indexes;
            var c1 = s1[i1.First()];
            var s2 = new Utf16String(utf16);
            var i2 = s2.Indexes;
            var c2 = s2[i2.First()];

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

            for (int n = 0; n < 3; n++)
            {
                Console.WriteLine("---- " + n + " ----");
                using (SW.New("  utf-8  code point: "))
                {
                    for (int i = 0; i < N; i++)
                        foreach (var c in new Utf8String(utf8))
                            ;
                }
                using (SW.New("  utf-16 code point: "))
                {
                    for (int i = 0; i < N; i++)
                        foreach (var c in new Utf16String(utf16))
                            ;
                }
                using (SW.New("  utf-8  index     : "))
                {
                    for (int i = 0; i < N; i++)
                    {
                        var x = new Utf8String(utf8);
                        foreach (var index in x.Indexes)
                        {
                            var c = x[index];
                        }
                    }
                }
                using (SW.New("  utf-16 index     : "))
                {
                    for (int i = 0; i < N; i++)
                    {
                        var x = new Utf16String(utf16);
                        foreach (var index in x.Indexes)
                        {
                            var c = x[index];
                        }
                    }
                }
            }
        }

        struct SW : IDisposable
        {
            private Stopwatch _sw;
            private string _caption;
            private SW(Stopwatch sw, string caption)
            {
                _sw = sw; sw.Start();
                _caption = caption;
            }
            public static SW New(string caption) => new SW(new Stopwatch(), caption);
            public void Dispose()
            {
                _sw.Stop();
                Console.WriteLine(_caption + _sw.Elapsed);
            }
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
