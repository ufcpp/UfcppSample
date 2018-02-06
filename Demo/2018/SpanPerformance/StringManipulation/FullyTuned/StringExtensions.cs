namespace StringManipulation.FullyTuned
{
    /// <summary>
    /// Split → Join な汎用的な書き方にこだわらず、ToCamel, ToSnake 専用に調整したもの。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// snake_case_string を CamelCaseString にする。
        /// </summary>
        public unsafe static string SnakeToCamel(this string snake)
        {
            var buf = stackalloc char[snake.Length];
            var count = 0;
            var init = true;

            for (int i = 0; i < snake.Length; i++)
            {
                var c = snake[i];
                if (c == '_')
                {
                    init = true;
                    continue;
                }

                if (init) buf[count++] = char.ToUpper(c);
                else buf[count++] = c;

                init = false;
            }

            return new string(buf, 0, count);
        }

        /// <summary>
        /// CamelCaseString を snake_case_string に変換する。
        /// </summary>
        public unsafe static string CamelToSnake(this string camel)
        {
            // 全部大文字の時がワーストケースで、元の長さの2倍。
            var buf = stackalloc char[camel.Length * 2];
            var count = 0;

            for (int i = 0; i < camel.Length; i++)
            {
                var c = camel[i];

                if (char.IsUpper(c))
                {
                    if (i != 0) buf[count++] = '_';
                    buf[count++] = char.ToLower(c);
                }
                else
                {
                    buf[count++] = c;
                }
            }

            return new string(buf, 0, count);
        }
    }
}
