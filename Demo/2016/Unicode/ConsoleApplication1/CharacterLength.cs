using System;
using System.Collections.Generic;
using System.Linq;
using static System.Text.Encoding;

namespace ConsoleApplication1
{
    class CharacterLength
    {
        public static void WriteLength()
        {
            WriteLength("aαあ😀");
            WriteLength("아조선글");
            WriteLength("👨‍👨‍👨‍👨‍👨‍👨‍👨");
            WriteLength("👨‍👩‍👦‍👦"); // 25バイト文字
            WriteLength("👨🏻‍👩🏿‍👦🏽‍👦🏼"); // 41バイト文字
            WriteLength("́");
            WriteLength("♢♠♤");
            WriteLength("🀄♔");
            WriteLength("☀☂☁");
            WriteLength("∀∂∋");
            WriteLength("ᚠᛃᚻ");
            WriteLength("𩸽");
        }

        static void WriteLength(string s)
        {
            var codes = GetCodePoints(s).ToArray();
            Console.WriteLine("文字: " + s);
            Console.WriteLine("UTF32 len: " + codes.Length);
            Console.WriteLine("UTF16 len: " + s.Length);
            Console.WriteLine("UTF8  len: " + UTF8.GetByteCount(s));

            foreach (var c in codes)
            {
                Console.WriteLine($"{c.value.ToString("X"),6} / {Join(c.utf32)} / {Join(c.utf16)} / {Join(c.utf8)}");
            }

            Console.Read();
        }

        private static string Join(byte[] codes) => string.Join(", ", codes.Select(x => x.ToString("X2")));
        private static string Join(int[] codes) => string.Join(", ", codes.Select(x => x.ToString("X4")));

        struct CodePoint
        {
            public int value;
            public string @char;
            public byte[] utf8;
            public byte[] utf16;
            public byte[] utf32;
        }

        static IEnumerable<CodePoint> GetCodePoints(string s)
        {
            var bytes = UTF32.GetBytes(s);

            for (int i = 0; i < bytes.Length; i += 4)
            {
                var cp = new CodePoint();

                var utf32 = new byte[4];
                Array.Copy(bytes, i, utf32, 0, 4);
                cp.utf32 = utf32;

                cp.value = (utf32[0] << 0)
                    | (utf32[1] << 8)
                    | (utf32[2] << 16)
                    | (utf32[3] << 24);

                var decoded = UTF32.GetString(utf32);
                cp.@char = decoded;
                cp.utf16 = Unicode.GetBytes(decoded);
                cp.utf8 = UTF8.GetBytes(decoded);

                yield return cp;
            }
        }
    }
}
