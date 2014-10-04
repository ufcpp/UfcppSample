using System.Collections.Generic;
using System.Linq;

namespace Palindrome
{
    class SimpleImplementation
    {
#if DEBUG
        public static int count;
#endif

        public static IEnumerable<string> GetPalindromes(string text)
        {
            return GetSubstrings(text).Where(IsPalindrome).Distinct();
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

        public static bool IsPalindrome(string s)
        {
            for (var i = 0; i < s.Length / 2; i++)
            {
#if DEBUG
                ++count;
#endif
                if (s[i] != s[s.Length - 1 - i])
                    return false;
            }
            return true;
        }
    }
}
