using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// C# 7.2/.NET Core 2.1 以降ならポインターを撤廃できる。
    /// 内部が unsafe なのはともかく、引数がポインターだと利用側に fixed ステートメントが必要でとてもじゃないけどやってられない。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 先頭だけ <see cref="char.ToUpper(char)"/>。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        /// <returns><paramref name="buffer"/>の書き込んだところ以降。</returns>
        public static Span<char> ToInitialUpper(ReadOnlySpan<char> s, Span<char> buffer)
        {
            buffer[0] = char.ToUpper(s[0]);
            s.Slice(1).CopyTo(buffer.Slice(1));
            return buffer.Slice(s.Length);
        }

        /// <summary>
        /// 先頭だけ <see cref="char.ToLower(char)"/>。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        /// <returns><paramref name="buffer"/>の書き込んだところ以降。</returns>
        public static Span<char> ToInitialLower(ReadOnlySpan<char> s, Span<char> buffer)
        {
            buffer[0] = char.ToLower(s[0]);
            s.Slice(1).CopyTo(buffer.Slice(1));
            return buffer.Slice(s.Length);
        }

        /// <summary>
        /// 先頭文字を大文字にする。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToInitialUpper(this string s)
        {
            if (s == null) return null;
            if (string.IsNullOrEmpty(s)) return string.Empty;

            Span<char> buf = stackalloc char[s.Length];
            ToInitialUpper(s, buf);
            return new string(buf);
        }

        /// <summary>
        /// 先頭文字を小文字にする。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToInitialLower(this string s)
        {
            if (s == null) return null;
            if (string.IsNullOrEmpty(s)) return string.Empty;


            Span<char> buf = stackalloc char[s.Length];
            ToInitialLower(s, buf);
            return new string(buf);
        }

        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public static string SnakeToCamel(this string snake)
        {
            Span<char> buf = stackalloc char[snake.Length];

            var p1 = buf;
            var len = 0;

            var splitter = new Splitter(snake, '_');
            while (splitter.TryMoveNext(out var s))
            {
                ToInitialUpper(s, p1);
                p1 = p1.Slice(s.Length);
                len += s.Length;
            }
            return new string(buf.Slice(0, len));
        }

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public static string CamelToSnake(this string camel)
        {
            Span<char> buf = stackalloc char[camel.Length * 2];

            var p1 = buf;
            var len = 0;

            var splitter = new UpperCaseSplitter(camel);
            bool first = true;
            while (splitter.TryMoveNext(out var s))
            {
                if (first) first = false;
                else
                {
                    p1[0] = '_';
                    p1 = p1.Slice(1);
                    ++len;
                }

                ToInitialLower(s, p1);
                p1 = p1.Slice(s.Length);
                len += s.Length;
            }
            return new string(buf.Slice(0, len));
        }
    }
}
