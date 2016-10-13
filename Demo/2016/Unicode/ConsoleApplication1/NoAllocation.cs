using System;
using System.Linq;
using static System.Text.Encoding;
using Utf16String = UtfString.Utf16.String;
using Utf8String = UtfString.Utf8.String;

namespace ConsoleApplication1
{
    class NoAllocation
    {
        public static void Test()
        {
            var testString = "aáαℵあáあ゙亜👩👩🏽";
            var s1 = new Utf8String(UTF8.GetBytes(testString));
            var s2 = new Utf16String(Unicode.GetBytes(testString));

            foreach (var c in s1) Console.WriteLine(c);
            foreach (var c in s2) Console.WriteLine(c);

            AssertIsTrue(s1.SequenceEqual(s2));
        }

        private static void AssertIsTrue(bool condition)
        {
            if (!condition) throw new InvalidOperationException();
        }
    }
}
