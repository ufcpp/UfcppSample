using System;
using System.Collections.Generic;
using System.Linq;

namespace Palindrome
{
    class Program
    {
        static void Main()
        {
            const string SampleText = "abcbad";

            Console.WriteLine("----- substrings -----");
            foreach (var x in GetSubstrings(SampleText))
            {
                Console.WriteLine(x);
            }

            Console.WriteLine("----- palindromes -----");
            foreach (var x in GetPalindromes(SampleText))
            {
                Console.WriteLine(x);
            }
        }

        static IEnumerable<string> GetPalindromes(string s)
        {
            return GetSubstrings(s).Where(IsPalindrome).Distinct();
        }

        static IEnumerable<string> GetSubstrings(string s)
        {
            for (var start = 0; start < s.Length; start++)
            {
                for (var length = 1; length <= s.Length - start; length++)
                {
                    yield return s.Substring(start, length);

                }
            }
        }

        static bool IsPalindrome(string s)
        {
            for (var i = 0; i < s.Length / 2; i++)
            {
                if (s[i] != s[s.Length - 1 - i])
                    return false;
            }
            return true;
        }
    }
}
