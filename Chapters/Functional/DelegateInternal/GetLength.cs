namespace DelegateInternal.GetLength
{
    using System;

    static class Program
    {
        static void M(Func<int> f)
        {
            Console.WriteLine(f());
        }

        static void Main()
        {
            var s = Console.ReadLine();

            // ラムダ式を利用(ちょっとだけ遅い)
            M(() => s.Length);

            // 拡張メソッドでカリー化(ちょっとだけ速い)
            M(s.GetLength);
        }

        static int GetLength(this string s) => s.Length;
    }
}
