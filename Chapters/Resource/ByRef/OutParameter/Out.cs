namespace ByRef.OutPerameter.Out
{
    using System;

    class Program
    {
        static void Main()
        {
            int a;
            Test(out a); // out を使った場合、変数を初期化しなくてもいい
            Console.Write("{0}\n", a);
        }

        static void Test(out int a)
        {
            a = 10; // out を使った場合、メソッド内で必ず値を代入しなければならない
        }
    }
}
