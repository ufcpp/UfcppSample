namespace LocalFunctions.CaptureOverwrite
{
    using System;

    class Program
    {
        static void Main()
        {
            var x = 1;

            // ローカル関数内で変数xを書き換え
            void f(int n) => x = n;

            Console.WriteLine(x); // 1

            f(2);
            Console.WriteLine(x); // 2
        }
    }
}
