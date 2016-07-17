namespace LocalFunctions.Factorial
{
    using System;

    class Program
    {
        static void Main()
        {
            // Main 関数の中で、ローカル関数 f を定義
            int f(int n) => n >= 1 ? n * f(n - 1) : 1;

            Console.WriteLine(f(10));
        }
    }
}
