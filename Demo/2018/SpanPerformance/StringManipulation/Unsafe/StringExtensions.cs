using System;

namespace StringManipulation.Unsafe
{
    /// <summary>
    /// <see cref="Classic.StringExtensions"/>だとヒープ確保しまくりなので、unsafe で最適化。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 先頭だけ <see cref="char.ToUpper(char)"/>。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        public unsafe static void ToInitialUpper(StringSpan s, char* buffer)
        {
            *buffer = char.ToUpper(*s.Pointer);
            var size = sizeof(char) * (s.Length - 1);
            Buffer.MemoryCopy(s.Pointer + 1, buffer + 1, size, size);
        }

        /// <summary>
        /// 先頭だけ <see cref="char.ToLower(char)"/>。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        public unsafe static void ToInitialLower(StringSpan s, char* buffer)
        {
            *buffer = char.ToLower(*s.Pointer);
            var size = sizeof(char) * (s.Length - 1);
            Buffer.MemoryCopy(s.Pointer + 1, buffer + 1, size, size);
        }

        /// <summary>
        /// 先頭文字を大文字にする。
        /// </summary>
        public unsafe static string ToInitialUpper(this string s)
        {
            if (s == null) return null;
            if (s.Length == 0) return "";

            var buffer = stackalloc char[s.Length];
            fixed (char* p = s) ToInitialUpper(new StringSpan(p, s.Length), buffer);
            return new string(buffer, 0, s.Length);
        }

        /// <summary>
        /// 先頭文字を小文字にする。
        /// </summary>
        public unsafe static string ToInitialLower(this string s)
        {
            if (s == null) return null;
            if (s.Length == 0) return "";

            var buffer = stackalloc char[s.Length];
            fixed (char* p = s) ToInitialLower(new StringSpan(p, s.Length), buffer);
            return new string(buffer, 0, s.Length);
        }

        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public unsafe static string SnakeToCamel(this string snake)
        {
            var buf = stackalloc char[snake.Length];

            var p1 = buf;
            fixed (char* p = snake)
            {
                var splitter = new Splitter(p, snake.Length, '_');
                while (splitter.TryMoveNext(out var s))
                {
                    ToInitialUpper(s, p1);
                    p1 += s.Length;
                }
            }
            var len = (int)(p1 - buf);
            return new string(buf, 0, len);
        }

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public unsafe static string CamelToSnake(this string camel)
        {
            var buf = stackalloc char[camel.Length * 2];

            var p1 = buf;
            fixed (char* p = camel)
            {
                var splitter = new UpperCaseSplitter(p, camel.Length);
                bool first = true;
                while (splitter.TryMoveNext(out var s))
                {
                    if (first) first = false;
                    else *p1++ = '_';

                    ToInitialLower(s, p1);
                    p1 += s.Length;
                }
            }
            var len = (int)(p1 - buf);
            return new string(buf, 0, len);
        }
    }
}
