namespace Keywords.Yield
{
    using System.Collections.Generic;

    class Program
    {
        static IEnumerator<yield> F()
        {
            // 「yield return」の2単語で初めてキーワードになる
            // 青いところだけがキーワード。
            // 水色が型名、黒が変数名。

            yield yield = 1;
            yield return yield;
        }

        struct yield
        {
            public int value;
            public static implicit operator yield(int n) => new yield { value = n };
        }
    }
}
