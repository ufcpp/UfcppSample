namespace LocalFunctions.Capture
{
    using System;
    using System.Linq;

    class Program
    {
        static void Main()
        {
            // ユーザーからの入力をローカル変数に記録
            var m = int.Parse(Console.ReadLine());
            var n = int.Parse(Console.ReadLine());

            var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // ユーザーの入力 m よりも大きいか判定
            bool filter(int x) => x > m;

            var output = input
                .Where(filter)
                .Select(x => n * x); // ユーザーの入力 n を掛ける

            foreach (var x in output)
            {
                Console.WriteLine(x);
            }
        }
    }
}
