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
        /// <paramref name="s"/>を<paramref name="splitter"/>で分割して<paramref name="formatter"/>で結合する。
        /// </summary>
        /// <param name="s">元の文字列。</param>
        /// <param name="buffer">書き込み先。</param>
        /// <param name="splitter">分割用。</param>
        /// <param name="formatter">結合用。</param>
        public static void Join<TSplitter, TFormatter>(this string s, ref Span<char> buffer, TSplitter splitter = default, TFormatter formatter = default)
            where TSplitter : IStringSplitter
            where TFormatter : IStringFormatter
        {
            ReadOnlySpan<char> span = s;

            while (splitter.TryMoveNext(ref span, out var word))
            {
                formatter.Write(word, ref buffer);
            }
        }

        static int Offset(Span<char> origin, Span<char> target)
            => (int)System.Runtime.CompilerServices.Unsafe.ByteOffset(
                ref System.Runtime.InteropServices.MemoryMarshal.GetReference(origin),
                ref System.Runtime.InteropServices.MemoryMarshal.GetReference(target))
            / sizeof(char);

        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public static string SnakeToCamel(this string snake)
        {
            Span<char> p = stackalloc char[snake.Length];
            var buffer = p;

            Join(snake, ref buffer, new Splitter('_'), new ToCamel());

            return new string(p.Slice(0, Offset(p, buffer)));
        }

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public static string CamelToSnake(this string camel)
        {
            // 全部大文字の時がワーストケースで、元の長さの2倍。
            Span<char> p = stackalloc char[camel.Length * 2];
            var buffer = p;

            Join(camel, ref buffer, new UpperCaseSplitter(), default(ToSnake));

            return new string(p.Slice(0, Offset(p, buffer)));
        }
    }
}
