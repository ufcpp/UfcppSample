using System.Collections.Generic;
using System.Linq;

namespace StringManipulation.Classic
{
    /// <summary>
    /// 古き良き実装。
    /// コードは割とシンプルだけど遅い。
    /// <see cref="string.Substring(int)"/>とか<see cref="string.Split(char[])"/>とか<see cref="string.Join(char, string[])"/>とかが一時オブジェクトをヒープ確保しまくり。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 先頭文字を大文字にする。
        /// </summary>
        public static string ToInitialUpper(this string s)
        {
            if (s == null) return null;
            if (string.IsNullOrEmpty(s)) return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 先頭文字を小文字にする。
        /// </summary>
        public static string ToInitialLower(this string s)
        {
            if (s == null) return null;
            if (string.IsNullOrEmpty(s)) return string.Empty;

            return char.ToLower(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public static string SnakeToCamel(this string snake) => string.Join("", snake.Split('_').Select(s => s.ToInitialUpper()));

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public static string CamelToSnake(this string camel) => string.Join("_", SplitByCase(camel).Select(x => x.ToInitialLower()));

        /// <summary>
        /// CamelCaseString を単語に分割。
        /// </summary>
        public static IEnumerable<string> SplitByCase(this string camel)
        {
            if (camel.Length == 0) yield break;

            var prev = 0;
            var i = 0;
            while (true)
            {
                prev = i;
                while (++i < camel.Length && !char.IsUpper(camel[i])) ;
                yield return camel.Substring(prev, i - prev);

                if (camel.Length == i) yield break;
            }
        }
    }
}
