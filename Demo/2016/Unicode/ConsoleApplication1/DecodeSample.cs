using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class DecodeSample
    {
        public static void Decode()
        {
            Decode("aáαあ😀");
        }

        public static void Decode(string s)
        {
            var utf8 = DecodeUtf8(s);
            var utf16 = DecodeUtf16(s);
            var utf32 = new UtfString.Utf32.String(Encoding.UTF32.GetBytes(s)).Select(x => x.Value);

            void write(IEnumerable<uint> codes) => Console.WriteLine(string.Join(" ", codes.Select(x => x.ToString("X2"))));

            write(utf8);
            write(utf16);
            write(utf32);
        }

        unsafe static IEnumerable<uint> DecodeUtf8(string s)
        {
            var buffer = Encoding.UTF8.GetBytes(s);
            return DecodeUtf8(buffer);
        }

        private static unsafe IEnumerable<uint> DecodeUtf8(byte[] buffer)
        {
            var list = new List<uint>();
            fixed (byte* begin = buffer)
            {
                var p = begin;
                var end = p + buffer.Length;
                while (p < end)
                {
                    var (cp, count) = DecodeUtf8(p);
                    list.Add(cp);
                    p += count;
                }
            }
            return list;
        }

        unsafe static IEnumerable<uint> DecodeUtf16(string s)
        {
            var buffer = Encoding.Unicode.GetBytes(s);
            return DecodeUtf16(buffer);
        }

        private static unsafe IEnumerable<uint> DecodeUtf16(byte[] buffer)
        {
            var list = new List<uint>();
            fixed (byte* begin = buffer)
            {
                var p = (ushort*)begin;
                var end = (ushort*)(begin + buffer.Length);
                while (p < end)
                {
                    var (cp, count) = DecodeUtf16(p);
                    list.Add(cp);
                    p += count;
                }
            }
            return list;
        }

        unsafe static (uint codePoint, int count) DecodeUtf8(byte* p)
        {
            uint code = p[0];

            if (code < 0b1100_0000)
            {
                // 1バイト: ASCII 文字
                return (code, 1);
            }
            if (code < 0b1110_0000)
            {
                // 2バイト: 主に欧米の文字
                code &= 0b1_1111;
                code = (code << 6) | (uint)(p[1] & 0b0011_1111);
                return (code, 2);
            }
            if (code < 0b1111_0000)
            {
                // 3バイト: かな漢字など
                code &= 0b1111;
                code = (code << 6) | (uint)(p[1] & 0b0011_1111);
                code = (code << 6) | (uint)(p[2] & 0b0011_1111);
                return (code, 3);
            }

            // 4バイト: 絵文字など
            code &= 0b0111;
            code = (code << 6) | (uint)(p[1] & 0b0011_1111);
            code = (code << 6) | (uint)(p[2] & 0b0011_1111);
            code = (code << 6) | (uint)(p[3] & 0b0011_1111);
            return (code, 4);
        }

        unsafe static (uint codePoint, int count) DecodeUtf16(ushort* p)
        {
            uint code = p[0];

            if ((code & 0b1111_1100_0000_0000) == 0b1101_1000_0000_0000)
            {
                // サロゲート ペア: 絵文字など
                code = (code & 0b0011_1111_1111) + 0b0100_0000;
                code = (code << 10) | ((uint)p[1] & 0b0011_1111_1111);
                return (code, 2);
            }
            else
            {
                // 頻度的に大半の文字がこっちに来るはず。バッファー内の値を素通し。
                return (code, 1);
            }
        }
    }
}
