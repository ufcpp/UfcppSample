namespace Patterns.UserDefinedOperator
{
    using System;

    class X
    {
        // 全てのインスタンスが等しいという挙動。
        // 当然、x == null も常に true。
        public static bool operator ==(X a, X b) => true;
        public static bool operator !=(X a, X b) => false;
    }

    class Program
    {
        static void Main()
        {
            var x = new X();

            // なんでも true なので、== null も true
            Console.WriteLine(x == null);

            // ユーザー定義の == は見ない。x が本当に null かどうかを見て、false になる
            Console.WriteLine(x is null);
        }
    }
}
