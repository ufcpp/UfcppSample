using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utf8String = UtfString.Utf8.String;
using Utf16String = UtfString.Utf16.String;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestUtfString
{
    [TestClass]
    public class TestString
    {
        private struct TestData
        {
            public string String { get; }
            public byte[] Utf8 { get; }
            public ushort[] Utf16 { get; }

            public TestData(string s)
            {
                String = s;
                Utf8 = Encoding.UTF8.GetBytes(s);
                Utf16 = Copy8To16(Encoding.Unicode.GetBytes(s));
            }

            private static ushort[] Copy8To16(byte[] encodedBytes)
            {
                if ((encodedBytes.Length % 2) != 0) throw new ArgumentException();
                var output = new ushort[encodedBytes.Length / 2];
                Buffer.BlockCopy(encodedBytes, 0, output, 0, encodedBytes.Length);
                return output;
            }
        }

        private static readonly IEnumerable<TestData> testData = new[]
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
        }.Select(s => new TestData(s)).ToArray();

        [TestMethod]
        public void ShouldBeIdentical()
        {
            foreach (var item in testData)
            {
                ShouldBeIdentical(item);
            }
        }

        private void ShouldBeIdentical(TestData s)
        {
            var utf8 = new Utf8String(s.Utf8);
            var cp8 = utf8.ToArray();
            var i8 = utf8.Indexes.ToArray();

            var utf16 = new Utf16String(s.Utf16);
            var cp16 = utf16.ToArray();
            var i16 = utf16.Indexes.ToArray();

            var info = CharacterInfo.GetCharacters(s.String).ToArray();

            Assert.AreEqual(info.Length, utf8.Length);
            Assert.AreEqual(info.Length, cp8.Length);
            Assert.AreEqual(info.Length, i8.Length);

            Assert.AreEqual(info.Length, utf16.Length);
            Assert.AreEqual(info.Length, cp16.Length);
            Assert.AreEqual(info.Length, i16.Length);

            for (int i = 0; i < info.Length; i++)
            {
                Assert.AreEqual((uint)info[i].codePoint, cp8[i].Value);
                Assert.AreEqual((uint)info[i].codePoint, utf8[i8[i]].Value);
                Assert.AreEqual((uint)info[i].codePoint, cp16[i].Value);
                Assert.AreEqual((uint)info[i].codePoint, utf16[i16[i]].Value);
            }
        }

        [TestMethod]
        public void NoAllocationWithForeach()
        {
            foreach (var item in testData)
            {
                NoAllocationWithForeach(item);
            }
        }

        private void NoAllocationWithForeach(TestData s)
        {
            const int N = 1000;

            {
                var start = GC.GetTotalMemory(false);

                var utf8 = new Utf8String(s.Utf8);
                for (int i = 0; i < N; i++)
                    foreach (var x in utf8)
                        ;

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }

            {
                var start = GC.GetTotalMemory(false);

                var utf8 = new Utf8String(s.Utf8);
                for (int i = 0; i < N; i++)
                    foreach (var x in utf8.Indexes)
                    {
                        var c = utf8[x];
                    }

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }

            {
                var start = GC.GetTotalMemory(false);

                var utf16 = new Utf16String(s.Utf16);
                for (int i = 0; i < N; i++)
                    foreach (var x in utf16)
                        ;

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }

            {
                var start = GC.GetTotalMemory(false);

                var utf16 = new Utf8String(s.Utf8);
                for (int i = 0; i < N; i++)
                    foreach (var x in utf16.Indexes)
                    {
                        var c = utf16[x];
                    }

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }
        }
    }
}
