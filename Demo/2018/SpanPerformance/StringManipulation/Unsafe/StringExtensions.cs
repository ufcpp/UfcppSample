namespace StringManipulation.Unsafe
{
    /// <summary>
    /// <see cref="Classic.StringExtensions"/>だとヒープ確保しまくりなので、unsafe で最適化。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// <paramref name="s"/>を<paramref name="splitter"/>で分割して<paramref name="formatter"/>で結合する。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        /// <param name="splitter">分割用。</param>
        /// <param name="formatter">結合用。</param>
        public unsafe static void Join<TSplitter, TFormatter>(this string s, ref StringSpan buffer, TSplitter splitter = default, TFormatter formatter = default)
            where TSplitter : IStringSplitter
            where TFormatter : IStringFormatter
        {
            fixed (char* p = s)
            {
                var span = new StringSpan(p, s.Length);
                while (splitter.TryMoveNext(ref span, out var word))
                {
                    formatter.Write(word, ref buffer);
                }
            }
        }

        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public unsafe static string SnakeToCamel(this string snake)
        {
            var p = stackalloc char[snake.Length];
            var buffer = new StringSpan(p, snake.Length);

            Join(snake, ref buffer, new Splitter('_'), new ToCamel());

            var len = (int)(buffer.Pointer - p);
            return new string(p, 0, len);
        }

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public unsafe static string CamelToSnake(this string camel)
        {
            // 全部大文字の時がワーストケースで、元の長さの2倍。
            var p = stackalloc char[camel.Length * 2];
            var buffer = new StringSpan(p, camel.Length);

            Join(camel, ref buffer, new UpperCaseSplitter(), default(ToSnake));

            var len = (int)(buffer.Pointer - p);
            return new string(p, 0, len);
        }
    }
}
