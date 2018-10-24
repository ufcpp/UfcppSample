using Xunit;
using System;
using System.Linq;
using UtfString.Slices;

namespace UnitTestUtfString
{
    public class TestSlicesUtf8String
    {
        [Fact]
        public void ShouldBeIdentical()
        {
            foreach (var s in TestData.Data)
            {
                ShouldBeIdenticalInternal(new Utf8String(s.Utf8), s.Utf32I);
            }
        }

        private static void ShouldBeIdenticalInternal(Utf8String s, uint[] expected)
        {
            var codePoints = s.CodePoints.ToArray();

            Assert.Equal(expected.Length, codePoints.Length);

            for (int i = 0; i < codePoints.Length; i++)
            {
                Assert.Equal(expected[i], codePoints[i].Value);
            }
        }

        [Fact]
        public void NoAllocationWithForeach()
        {
            const int N = 1000;

            foreach (var s in TestData.Data)
            {
                NoAllocationWithForeachInternal(new Utf8String(s.Utf8), N);
            }
        }

        private static void NoAllocationWithForeachInternal(Utf8String s, int n)
        {
            var start = GC.GetTotalMemory(false);

            for (int i = 0; i < n; i++)
            {
                foreach (var x in s.CodePoints)
                    ;

                if (s.Length > 3)
                {
                    var sub1 = s.Substring(1, 1);
                    var sub2 = s.Substring(2);
                    var sub3 = s.Substring(3);
                }
            }

            var end = GC.GetTotalMemory(false);

            Assert.Equal(start, end);
        }
    }
}
