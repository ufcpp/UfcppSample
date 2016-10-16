using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UtfString;
using UtfString.Slices;

namespace UnitTestUtfString
{
    [TestClass]
    public class TestSlicesUtf8String
    {
        [TestMethod]
        public void ShouldBeIdentical()
        {
            foreach (var s in TestData.Data)
            {
                ShouldBeIdentical(new Utf8String(s.Utf8), s.Utf32I);
            }
        }

        private static void ShouldBeIdentical(Utf8String s, uint[] expected)
        {
            var codePoints = ((IEnumerable<CodePoint>)s).ToArray();

            Assert.AreEqual(expected.Length, codePoints.Length);

            for (int i = 0; i < codePoints.Length; i++)
            {
                Assert.AreEqual(expected[i], codePoints[i].Value);
            }
        }

        [TestMethod]
        public void NoAllocationWithForeach()
        {
            const int N = 1000;

            foreach (var s in TestData.Data)
            {
                NoAllocationWithForeach(new Utf8String(s.Utf8), N);
            }
        }

        private static void NoAllocationWithForeach(Utf8String s, int n)
        {
            var start = GC.GetTotalMemory(false);

            for (int i = 0; i < n; i++)
                foreach (var x in s)
                    ;

            var end = GC.GetTotalMemory(false);

            Assert.AreEqual(start, end);
        }
    }
}
