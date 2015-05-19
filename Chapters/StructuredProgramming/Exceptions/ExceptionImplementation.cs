using System;

namespace Exceptions
{
    /// <summary>
    /// 不正な入力に対して例外を出す実装。
    /// </summary>
    /// <remarks>
    /// 「不正な入力をはじくのはメソッドの手前で、メソッドを呼ぶ側が責任を持ってやれ」という場合はこういう実装にする。
    /// 実のところ、この手の例外はあんまりcatchしない。「例外が出る状況 = バグってる」というタイプの例外。デバッグで取りきる。
    /// </remarks>
    class ExceptionImplementation
    {
        public static void Main()
        {
            try
            {
                Console.Write("{0}\n", StringToInt("12345"));
                Console.Write("{0}\n", StringToInt("12a45"));
                //↑ ここで FormatException 例外が投げられる。
            }
            catch (FormatException)
            {
                Console.Write("想定外の文字列が入力されました");
            }
        }

        /// <summary>
        /// 文字→整数
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static int CharToInt(char c)
        {
            if (c < '0' || '9' < c)
                throw new FormatException(); // 不正な文字が入力された場合、例外を投げる

            return c - '0';
        }

        /// <summary>
        /// 文字列→整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static int StringToInt(string str)
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
