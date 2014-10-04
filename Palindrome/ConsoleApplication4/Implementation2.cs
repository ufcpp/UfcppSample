using System.Collections.Generic;
using System.Linq;

namespace Palindrome
{
    class Implementation2
    {
#if DEBUG
        public static int count;
#endif
        public static IEnumerable<string> GetSubstrings(string text)
        {
            // 奇数
            for (var middleIndex = 0; middleIndex < text.Length; middleIndex++)
            {
                for (int i = 0; middleIndex - i >= 0 && middleIndex + i < text.Length; i++)
                {
                    yield return text.Substring(middleIndex - i, 2 * i + 1);
                }
            }

            // 偶数
            for (var middleIndex = 0; middleIndex < text.Length - 1; middleIndex++)
            {
                for (int i = 0; middleIndex - i >= 0 && middleIndex + i + 1 < text.Length; i++)
                {
                    yield return text.Substring(middleIndex - i, 2 * i + 2);
                }
            }
        }

        public static IEnumerable<string> GetPalindromes(string text)
        {
            return GetPalindromesInternal(text).Distinct();
        }

        public static IEnumerable<string> GetPalindromesInternal(string text)
        {
            // 奇数
            for (var middleIndex = 0; middleIndex < text.Length; middleIndex++)
            {
                for (var i = 0; ; i++)
                {
#if DEBUG
                    ++count;
#endif
                    var start = middleIndex - i;
                    var length = 2 * i + 1;
                    var last = start + length - 1;

                    if (start < 0 || last >= text.Length)
                        break;
                    if (text[start] != text[start + length - 1])
                        break;
                    yield return text.Substring(start, length);
                }
            }

            // 偶数
            for (var middleIndex = 0; middleIndex < text.Length - 1; middleIndex++)
            {
                for (var i = 0; ; i++)
                {
#if DEBUG
                    ++count;
#endif
                    var start = middleIndex - i;
                    var length = 2 * i + 2;
                    var last = start + length - 1;

                    if (start < 0 || last >= text.Length)
                        break;
                    if (text[start] != text[start + length - 1])
                        break;
                    yield return text.Substring(start, length);
                }
            }
        }
    }
}
