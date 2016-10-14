using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utf8String = UtfString.Utf8.String;
using Utf16StringA = UtfString.ArrayImplementation.Utf16.String;
using Utf16StringU = UtfString.Unsafe.Utf16.String;
using Utf32String = UtfString.Unsafe.Utf32.String;
using CompactString = UtfString.Unsafe.DualEncoding.String;
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
            ShouldBeIdentical1(new Utf16StringA(s.Utf16S), s.Utf32I);
            ShouldBeIdentical1(new Utf16StringU(s.Utf16B), s.Utf32I);
            ShouldBeIdentical1(new Utf32String(s.Utf32B), s.Utf32I);

            ShouldBeIdentical1(new CompactString(true, s.Utf16B), s.Utf32I);
            if (s.Latin1 != null)
            {
                ShouldBeIdentical1(new CompactString(false, s.Latin1), s.Utf32I);
            }
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
            foreach (var item in TestData.Data)
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

            NoAllocationWithForeach(new CompactString(true, s.Utf16B), N);
            if (s.Latin1 != null)
            {
                NoAllocationWithForeach(new CompactString(false, s.Latin1), N);
            }
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
