using System;
using System.Collections.Generic;
using System.Linq;

namespace Palindrome
{
    class Implementation1
    {
#if DEBUG
        public static int count;
#endif

        public static IEnumerable<string> GetPalindromes(string text)
        {
            return GetSubstrings(text, IsPalindrome).Distinct();
        }

        public static IEnumerable<string> GetSubstrings(string text)
        {
            for (var start = 0; start < text.Length; start++)
            {
                for (var length = 1; length <= text.Length - start; length++)
                {
                    yield return text.Substring(start, length);
                }
            }
        }

        private delegate bool SubstringPredicate(string text, int startIndex, int length);

        private static IEnumerable<string> GetSubstrings(string text, SubstringPredicate condition)
        {
            for (var start = 0; start < text.Length; start++)
            {
                for (var length = 1; length <= text.Length - start; length++)
                {
                    if (condition(text, start, length)) // 判定ではじいてから Substring
                        yield return text.Substring(start, length);
                }
            }
        }

        public static bool IsPalindrome(string text, int startIndex, int length)
        {
            for (var i = 0; i < length / 2; i++)
            {
#if DEBUG
                ++count;
#endif
                if (text[i + startIndex] != text[startIndex + length - 1 - i])
                    return false;
            }
            return true;
        }
    }
}
