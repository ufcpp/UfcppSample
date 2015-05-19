using System;

namespace Exceptions
{
    /// <summary>
    /// エラーがあるときには特定の値を返すような実装。
    /// この場合、「戻り値は必ず正」という縛りの中だから、-1 とかを使える。
    /// この他、「int? 型にしておいて、不正な入力の場合には null を返す」とかもよくやる。
    /// </summary>
    /// <remarks>
    /// この手の、文字列を整数化するような場合、<see cref="int.TryParse(string, out int)"/> みたいな、bool戻り値を使う場合もよくある。
    /// </remarks>
    class ReturnCodeImplementation
    {
        public static void Main()
        {
            int i;

            i = StringToInt("12345");
            if (i == -1)
                Console.Write("想定外の文字列が入力されました");
            else
                Console.Write("{0}\n", i);

            i = StringToInt("12a45");
            if (i == -1)
                Console.Write("想定外の文字列が入力されました");
            else
                Console.Write("{0}\n", i);
        }

        /// <summary>
        /// 文字→整数
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CharToInt(char c)
        {
            if ('0' <= c && c <= '9')
                return c - '0';
            else
                return -1; // 想定外の文字が入力された場合、-1 を返す。
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
                if (i == -1) return -1; // 想定外の文字列が入力された場合、-1 を返す。
                val = val * 10 + i;
            }
            return val;
        }
    }
}
