using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utf8String = UtfString.Utf8.String;
using Utf16String = UtfString.Utf16.String;
using static System.Text.Encoding;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestUtfString
{
    [TestClass]
    public class TestString
    {
        private static readonly IEnumerable<string> testData = new[]
        {
            "",
            "aáαℵあáあ゙亜👩👩🏽",
            "aαあ😀",
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
        };

        [TestMethod]
        public void ShouldBeIdentical()
        {
            foreach (var item in testData)
            {
                ShouldBeIdentical(item);
            }
        }

        private void ShouldBeIdentical(String s)
        {
            var utf8 = new Utf8String(UTF8.GetBytes(s));
            var cp8 = utf8.ToArray();

            var utf16 = new Utf16String(Unicode.GetBytes(s));
            var cp16 = utf16.ToArray();

            var info = CharacterInfo.GetCharacters(s).ToArray();

            Assert.AreEqual(info.Length, cp8.Length);
            Assert.AreEqual(info.Length, cp16.Length);

            for (int i = 0; i < info.Length; i++)
            {
                Assert.AreEqual((uint)info[i].codePoint, cp8[i].Value);
                Assert.AreEqual((uint)info[i].codePoint, cp16[i].Value);
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

        private void NoAllocationWithForeach(string s)
        {
            const int N = 1000;
            var utf8 = new Utf8String(UTF8.GetBytes(s));

            {
                var start = GC.GetTotalMemory(false);

                for (int i = 0; i < N; i++)
                    foreach (var x in utf8)
                        ;

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }

            var utf16 = new Utf16String(Unicode.GetBytes(s));

            {
                var start = GC.GetTotalMemory(false);

                for (int i = 0; i < N; i++)
                    foreach (var x in utf16)
                        ;

                var end = GC.GetTotalMemory(false);

                Assert.AreEqual(start, end);
            }
        }
    }
}
