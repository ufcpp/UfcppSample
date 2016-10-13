using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utf8String = UtfString.Utf8.String;
using Utf16StringA = UtfString.ArrayImplementation.Utf16.String;
using Utf16StringU = UtfString.Unsafe.Utf16.String;
using Utf32String = UtfString.Unsafe.Utf32.String;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UtfString;

namespace UnitTestUtfString
{
    [TestClass]
    public class TestString
    {
        private struct TestData
        {
            public string String { get; }
            public byte[] Utf8 { get; }
            public byte[] Utf16B { get; }
            public ushort[] Utf16S { get; }
            public byte[] Utf32B { get; }
            public uint[] Utf32I { get; }

            public TestData(string s)
            {
                String = s;
                Utf8 = Encoding.UTF8.GetBytes(s);
                Utf16B = Encoding.Unicode.GetBytes(s);
                Utf16S = Copy8To16(Utf16B);
                Utf32B = Encoding.UTF32.GetBytes(s);
                Utf32I = Copy8To32(Utf32B);
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
            ShouldBeIdentical1(new Utf8String(s.Utf8), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringA(s.Utf16S), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringU(s.Utf16B), s.Utf32I);
            ShouldBeIdentical1(new Utf32String(s.Utf32B), s.Utf32I);
        }

        private static void ShouldBeIdentical1<TIndex, TEnumerator, TIndexEnumerable, TIndexEnumerator>(IString<TIndex, TEnumerator, TIndexEnumerable, TIndexEnumerator> s, uint[] expected)
            where TIndex : struct
            where TEnumerator : struct, IEnumerator<CodePoint>
            where TIndexEnumerator : struct, IEnumerator<TIndex>
            where TIndexEnumerable : struct, IIndexEnumerable<TIndex, TIndexEnumerator>
        {
            var codePoints = ((IEnumerable<CodePoint>)s).ToArray();
            var indexes = ((IEnumerable<TIndex>)s.Indexes).ToArray();

            Assert.AreEqual(codePoints.Length, indexes.Length);

            for (int i = 0; i < codePoints.Length; i++)
            {
                Assert.AreEqual(codePoints[i], s[indexes[i]]);
                Assert.AreEqual(codePoints[i].Value, expected[i]);
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

            NoAllocationWithForeach(new Utf8String(s.Utf8), N);
            NoAllocationWithForeach(new Utf16StringA(s.Utf16S), N);
            NoAllocationWithForeach(new Utf16StringU(s.Utf16B), N);
            NoAllocationWithForeach(new Utf32String(s.Utf32B), N);
        }

        private static void NoAllocationWithForeach<TIndex, TEnumerator, TIndexEnumerable, TIndexEnumerator>(IString<TIndex, TEnumerator, TIndexEnumerable, TIndexEnumerator> s, int n)
            where TIndex : struct
            where TEnumerator : struct, IEnumerator<CodePoint>
            where TIndexEnumerator : struct, IEnumerator<TIndex>
            where TIndexEnumerable : struct, IIndexEnumerable<TIndex, TIndexEnumerator>
        {
            var start = GC.GetTotalMemory(false);

            for (int i = 0; i < n; i++)
                foreach (var x in s)
                    ;

            for (int i = 0; i < n; i++)
                foreach (var x in s.Indexes)
                {
                    var c = s[x];
                }

            var end = GC.GetTotalMemory(false);

            Assert.AreEqual(start, end);
        }
    }
}
