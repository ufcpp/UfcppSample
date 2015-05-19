using System;

namespace Exceptions
{
    /// <summary>
    /// 不正な文字列とか気にせず、変な値が返ってくるのを許容する実装。
    /// 利用者側が気を付けて使ってられる範囲なら案外ありな実装。
    /// 大規模化・汎用化してきたらはまる。
    /// </summary>
    class IgnoreErrorImplementation
    {
        public static void Main()
        {
            Console.Write("{0}\n", StringToInt("12345"));
            Console.Write("{0}\n", StringToInt("12a45")); // 途中に数字以外の文字が
        }

        /// <summary>
        /// 文字→整数
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CharToInt(char c)
        {
            return c - '0';
        }

        /// <summary>
        /// 文字列→整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StringToInt(string str)
        {
            int val = 0;
            foreach (char c in str)
            {
                int i = CharToInt(c);
                val = val * 10 + i;
            }
            return val;
        }
    }
}
