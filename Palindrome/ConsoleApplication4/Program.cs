using System;
using System.Diagnostics;
using System.Linq;

namespace Palindrome
{
    class Program
    {
        static void Main()
        {
            var r = new Random();

            var sampleData = new[]
            {
                new { Tag = "手打ちデータ1", Text = "abcbad" },
                new { Tag = "手打ちデータ2", Text = "aaaaaaaaaabowijaoiwejroasafasdfasaaaaabbbbcbbbbaaoiwejatow" },
                new { Tag = "手打ちデータ3", Text = "oe4itjaoewrgosergoishjreioushzudfizasdnfkjdnfvlmxoisrejaoweshgtriuaerngkjrsnrkljsnfkdlmszrhgaiuewhrtiouarehnpinrmxlkndl" },
                // アルファベット（26文字）想定
                new { Tag = "26種500文字", Text = CreateText(r, 500, 26) },
                new { Tag = "同1000文字", Text = CreateText(r, 1000, 26) },
                new { Tag = "同2000文字", Text = CreateText(r, 2000, 26) },
                new { Tag = "同4000文字", Text = CreateText(r, 4000, 26) },
                // 全部同じ文字 = おそらくワーストケース O(n^3)
                new { Tag = "全部同じ100文字", Text = CreateText(r, 100, 1) },
                new { Tag = "同200文字", Text = CreateText(r, 200, 1) },
                new { Tag = "同400文字", Text = CreateText(r, 400, 1) },
                new { Tag = "同800文字", Text = CreateText(r, 800, 1) },
            };

            foreach (var data in sampleData)
            {
                Console.WriteLine(data.Tag);
                //TestGetSubstrings(data.Text);
                TestGetPalindromes(data.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="random"></param>
        /// <param name="length">生成したいテキストの文字数。</param>
        /// <param name="charCount">文字の種類の数。</param>
        /// <returns></returns>
        static string CreateText(Random random, int length, int charCount)
        {
            var c = Enumerable.Range(0, length).Select(_ => (char)('a' + random.Next(0, charCount)));
            return new string(c.ToArray());
        }

        private static void TestGetSubstrings(string text)
        {
            var expected = SimpleImplementation.GetSubstrings(text).ToArray();
            var actual = Implementation2.GetSubstrings(text).ToArray();

            if (!expected.OrderBy(x => x).SequenceEqual(actual.OrderBy(x => x)))
            {
                Console.WriteLine("error GetSbustrings");
            }
        }

        private static void TestGetPalindromes(string text)
        {
            var sw = new Stopwatch();
            Implementation1.count = 0;
            sw.Restart();
            var expected = Implementation1.GetPalindromes(text).ToArray();
            sw.Stop();
            var simpleCount = Implementation1.count;
            var simpleTime = sw.Elapsed;

            Implementation2.count = 0;
            sw.Restart();
            var actual = Implementation2.GetPalindromes(text).ToArray();
            sw.Stop();
            var newCount = Implementation2.count;
            var newTime = sw.Elapsed;

            Console.WriteLine("べた実装 {0}, {2}. 新実装 {1}, {3}", simpleCount, newCount, simpleTime, newTime);

            if (!expected.OrderBy(x => x).SequenceEqual(actual.OrderBy(x => x)))
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