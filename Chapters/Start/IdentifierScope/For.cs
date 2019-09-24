namespace IdentifierScope.For
{
    using System;

    public class Program
    {
        public static void M(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(i);
            }

            {
                int i = 0;
                while(i < n)
                {
                    Console.WriteLine(i);
                    i++;
                }
            }
        }

        public static void M1()
        {
            Action a = null!;

            for (int i = 0; i < 10; i++)
            {
                a += () => Console.WriteLine(i); // この i はずっと共有
            }
            // ループを抜けたときには、i の値は 10 に置き換わってる

            // 結果、10が10回表示される
            a();
        }

        public static void M2()
        {
            Action a = null!;

            for (int i = 0; i < 10; i++)
            {
                var j = i;
                a += () => Console.WriteLine(j); // この j は1回1回別
            }

            // 結果、0～9が1回ずつ表示される
            a();
        }
    }
}
