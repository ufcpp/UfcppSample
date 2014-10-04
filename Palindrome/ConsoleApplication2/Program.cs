using System;
using System.Collections.Generic;
using System.Linq;

namespace Palindrome
{
    class Program
    {
        static void Main()
        {
            var sampleData = new[]
            {
                "abcbad",
                "aaaaaaaaaabowijaoiwejroasafasdfasaaaaabbbbcbbbbaaoiwejatow",
                "oe4itjaoewrgosergoishjreioushzudfizasdnfkjdnfvlmxoisrejaoweshgtriuaerngkjrsnrkljsnfkdlmszrhgaiuewhrtiouarehnpinrmxlkndl",
            };

            foreach (var data in sampleData)
            {
                TestGetSubstrings(data);
                TestGetPalindromes(data);
            }
        }

        private static void TestGetSubstrings(string text)
        {
            var expected = SimpleImplementation.GetSubstrings(text).ToArray();
            var actual = Implementation1.GetSubstrings(text).ToArray();

            if (!expected.OrderBy(x => x).SequenceEqual(actual.OrderBy(x => x)))
            {
                Console.WriteLine("error GetSbustrings");
            }
        }

        private static void TestGetPalindromes(string text)
        {
            var expected = SimpleImplementation.GetPalindromes(text).OrderBy(x => x).ToArray();
            var actual = Implementation1.GetPalindromes(text).OrderBy(x => x).ToArray();

            if (!expected.SequenceEqual(actual))
            {
                Console.WriteLine("error GetPalindromes");

                Console.WriteLine(string.Join(", ", expected));
                Console.WriteLine(string.Join(", ", actual));
            }
        }

        private static void Write(string text)
        {
            Console.WriteLine("----- substrings -----");
            foreach (var x in Implementation1.GetSubstrings(text))
            {
                Console.WriteLine(x);
            }

            Console.WriteLine("----- palindromes -----");
            foreach (var x in Implementation1.GetPalindromes(text))
            {
                Console.WriteLine(x);
            }
        }

    }
}