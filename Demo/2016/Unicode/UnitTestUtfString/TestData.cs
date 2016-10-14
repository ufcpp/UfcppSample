using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestUtfString
{
    internal struct TestData
    {
        public static readonly IEnumerable<TestData> Data = new[]
        {
            "aáαあ😀",
            "aáαℵあáあ゙亜👩👩🏽",
            "아조선글",
            "👨‍👨‍👨‍👨‍👨‍👨‍👨",
            "👨‍👩‍👦‍👦",
            "👨🏻‍👩🏿‍👦🏽‍👦🏼",
            "́",
            "♢♠♤",
            "🀄♔",
            "☀☂☁",
            "∀∂∋",
            "ᚠᛃᚻ",
            "𩸽",
            "",
            "ascii string !\"#$%&'() 1234567890 AQWSEDRFTGYHUJIKOLP+@,./\\<>?_",
            "latin1 string °±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ",
        }.Select(s => new TestData(s)).ToArray();


        public string String { get; }
        public byte[] Utf8 { get; }
        public byte[] Utf16B { get; }
        public ushort[] Utf16S { get; }
        public byte[] Utf32B { get; }
        public uint[] Utf32I { get; }
        public byte[] Latin1 { get; }

        public TestData(string s)
        {
            String = s;
            Utf8 = Encoding.UTF8.GetBytes(s);
            Utf16B = Encoding.Unicode.GetBytes(s);
            Utf16S = Copy8To16(Utf16B);
            Utf32B = Encoding.UTF32.GetBytes(s);
            Utf32I = Copy8To32(Utf32B);

            if (s.All(c => c < 0x100))
                Latin1 = Encoding.GetEncoding("iso-8859-1").GetBytes(s);
            else
                Latin1 = null;
        }

        private static ushort[] Copy8To16(byte[] encodedBytes)
        {
            if ((encodedBytes.Length % 2) != 0) throw new ArgumentException();
            var output = new ushort[encodedBytes.Length / 2];
            Buffer.BlockCopy(encodedBytes, 0, output, 0, encodedBytes.Length);
            return output;
        }

        private static uint[] Copy8To32(byte[] encodedBytes)
        {
            if ((encodedBytes.Length % 4) != 0) throw new ArgumentException();
            var output = new uint[encodedBytes.Length / 4];
            Buffer.BlockCopy(encodedBytes, 0, output, 0, encodedBytes.Length);
            return output;
        }
    }
}
