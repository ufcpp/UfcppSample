using System;

namespace DefiniteAssignment
{
    class Program
    {
#if ERROR
        static void M1()
        {
            string s;
            // 初期化しないまま s を使ったのでエラー。
            Console.WriteLine(s.Length);
        }
#endif
        static void M2()
        {
            string s;
            if (true) s = "abc";
            // エラーなし
            Console.WriteLine(s.Length);
        }

#if ERROR
        static void M3()
        {
            string s;
            if (false) s = "abc";
            // エラーあり
            Console.WriteLine(s.Length);
        }

        static void M(bool flag)
        {
            string s;
            if (flag) s = "abc";
            // エラーあり
            Console.WriteLine(s.Length);
        }
#endif

        static void M2(bool flag)
        {
            string s;
            if (flag) s = "abc";
            else s = "def";
            // エラーなし
            Console.WriteLine(s.Length);
        }
    }
}
