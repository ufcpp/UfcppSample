//#define GENERIC

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utf8StringA = UtfString.ArrayImplementation.Utf8.String;
using Utf16StringA = UtfString.ArrayImplementation.Utf16.String;
using Utf8String = UtfString.Utf8.String;
using Utf16StringU = UtfString.Utf16.String;
using Utf32String = UtfString.Utf32.String;
using CompactString = UtfString.DualEncoding.String;
#if GENERIC
using Utf8StringG = UtfString.Generic.String<byte, UtfString.Generic.ByteAccessor, UtfString.Generic.Utf8Decoder>;
using Utf16StringG = UtfString.Generic.String<ushort, UtfString.Generic.ShortAccessor, UtfString.Generic.Utf16Decoder<UtfString.Generic.ShortAccessor>>;
using CompactStringG = UtfString.Generic.String<ushort, UtfString.Generic.DualAccessor, UtfString.Generic.Utf16Decoder<UtfString.Generic.DualAccessor>>;
using Utf32StringG = UtfString.Generic.String<uint, UtfString.Generic.IntAccessor, UtfString.Generic.Utf32Decoder>;
#endif
using System.Collections.Generic;
using System.Linq;
using UtfString;

namespace UnitTestUtfString
{
    [TestClass]
    public partial class TestString
    {
        [TestMethod]
        public void ShouldBeIdentical()
        {
            foreach (var item in TestData.Data)
            {
                ShouldBeIdentical(item);
            }
        }

        private void ShouldBeIdentical(TestData s)
        {
            ShouldBeIdentical1(new Utf8String(s.Utf8), s.Utf32I);
            ShouldBeIdentical1(new Utf8StringA(s.Utf8), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringA(s.Utf16S), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringU(s.Utf16B), s.Utf32I);
            ShouldBeIdentical1(new Utf32String(s.Utf32B), s.Utf32I);

            ShouldBeIdentical1(new CompactString(true, s.Utf16B), s.Utf32I);
            if (s.Latin1 != null)
            {
                ShouldBeIdentical1(new CompactString(false, s.Latin1), s.Utf32I);
            }

#if GENERIC
            ShouldBeIdentical1(new Utf8StringG(s.Utf8), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringG(s.Utf16B), s.Utf32I);
            ShouldBeIdentical1(new Utf32StringG(s.Utf32B), s.Utf32I);
            ShouldBeIdentical1(new CompactStringG(s.Utf16B), s.Utf32I);
            if (s.Latin1 != null)
            {
                ShouldBeIdentical1(new CompactStringG((false, s.Latin1)), s.Utf32I);
            }
#endif
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
                Assert.AreEqual(expected[i], codePoints[i].Value);
                Assert.AreEqual(expected[i], s[indexes[i]].Value);
                Assert.AreEqual(codePoints[i], s[indexes[i]]);
            }
        }

        [TestMethod]
        public void NoAllocationWithForeach()
        {
            foreach (var item in TestData.Data)
            {
                NoAllocationWithForeach(item);
            }
        }

        private void NoAllocationWithForeach(TestData s)
        {
            const int N = 1000;

            NoAllocationWithForeach(new Utf8String(s.Utf8), N);
            NoAllocationWithForeach(new Utf8StringA(s.Utf8), N);
            NoAllocationWithForeach(new Utf16StringA(s.Utf16S), N);
            NoAllocationWithForeach(new Utf16StringU(s.Utf16B), N);
            NoAllocationWithForeach(new Utf32String(s.Utf32B), N);

            NoAllocationWithForeach(new CompactString(true, s.Utf16B), N);
            if (s.Latin1 != null)
            {
                NoAllocationWithForeach(new CompactString(false, s.Latin1), N);
            }

#if GENERIC
            NoAllocationWithForeach(new Utf8StringG(s.Utf8), N);
            NoAllocationWithForeach(new Utf16StringG(s.Utf16B), N);
            NoAllocationWithForeach(new Utf32StringG(s.Utf32B), N);
            NoAllocationWithForeach(new CompactStringG(s.Utf16B), N);
            if (s.Latin1 != null)
            {
                NoAllocationWithForeach(new CompactStringG((false, s.Latin1)), N);
            }
#endif
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
