using System;

namespace ConsoleApplication1
{
    /// <summary>
    /// 結合文字(Mn, Mcカテゴリーの文字)や、formatting-character を識別子に使えることをいろいろ悪用。
    /// </summary>
    class GraphemeIdentifiers
    {
        /// <summary>
        /// Cf 文字を識別子に使った場合、削除するらしい。
        /// ab と a\u200db (\u200d は zero witdh joiner)は同じ識別子として扱われる。
        /// </summary>
        public static void ZeroWidthJoiner()
        {
            var ab = 0;
            a\u200db = 1; // ab と同じ扱い
            Console.WriteLine(ab); // 1

            Console.WriteLine(nameof(a\u200db).Length); // 2。つまり、"ab" 扱い

            var s1 = "ab";
            var s2 = "a\u200db";
            Console.WriteLine(s1.Length); // 2
            Console.WriteLine(s2.Length); // 3
            Console.WriteLine(s1 == s2);  // false
        }

        /// <summary>
        /// non-spacing mark(アクセント記号とか濁点とか)を使うと、
        /// 見た目が全く同じで違う文字ってのが作れる。
        /// </summary>
        public static void DiacriticalMark()
        {
            var á = 1; // U+0061 U+0301
            var á = 2; // U+00E1

            Console.WriteLine(á); // 1
            Console.WriteLine(á); // 2

            var が = 1; // U+304B U+3099
            var が = 2; // U+304C

            Console.WriteLine(が); // 1
            Console.WriteLine(が); // 2
        }

        public static void IdeographicVariationSelector()
        {
        }
    }
}
