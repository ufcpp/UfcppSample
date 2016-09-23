namespace Keywords.NameOf
{
    using System;

    class NoMethod
    {
        static void F()
        {
            // nameof メソッドが存在しないのでこれはキーワード
            var x = 1;
            Console.WriteLine(nameof(x)); // x
        }
    }

    class SuccessfullyCompiled
    {
        static void F()
        {
            // nameof メソッドがあるのでそちらが呼ばれてしまう
            var x = 1;
            Console.WriteLine(nameof(x)); // abc
        }

        static string nameof(int n) => "abc";
    }

    class Erroneous
    {
        static void F()
        {
            // nameof メソッドがある上に、型が合わない
            // コンパイル エラーになる
#if false
            var x = 1;
            Console.WriteLine(nameof(x));
#endif
        }

        static string nameof(string s) => "";
    }
}
