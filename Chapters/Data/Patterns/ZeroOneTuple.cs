namespace Patterns.ZeroOneTuple
{
    using System;

    class X
    {
        public void Deconstruct() { }
        public void Deconstruct(out int a) => a = 0;
    }

    class Program
    {
        static void Main() => M(new X());

        static void M(X x)
        {
            // 0 引数の位置パターン。
            // Deconstruct() を持っていることが使える条件。
            if (x is ()) Console.WriteLine("Deconstruct()");

            // 1 引数の位置パターン。
            // Deconstruct(out T) を持っていることが使える条件。
            // ただ、キャストの () との区別が難しいらしく、素直に x is (int a) とは書けない。
            // 前後に余計な var や _ を付ける必要あり。
            if (x is (int a) _) Console.WriteLine($"Deconstruct({a})");
        }
    }
}
