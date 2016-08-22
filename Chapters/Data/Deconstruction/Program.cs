using System;
using System.Collections.Generic;

namespace Deconstruction
{
    class Program
    {
        static void Main(string[] args)
        {
            MeaninglessVariable();
            ManuallyDeconstructed();

            var items = new[] { 1, 2, 3, 4, 5 };

            DeconstractionDeclaration(items);
            DeconstractionAssingment(items);
            DeconstractionAssingment();
            Conversion();
        }

        private static void Conversion()
        {
            // Tally の戻り値は (count, sum) の順
            var t = Tally(new[] { 1, 2, 3, 4, 5 });

            // sum = t.count, count = t.sum の意味になるので注意が必要
            (int sum, int count) = t;
            Console.WriteLine(sum);   // 5
            Console.WriteLine(count); // 15

            // int → object も int → long も暗黙的に変換可能
            // なので、分解もでもこの変換が暗黙的に可能
            (object x, long y) = t;
        }

        private static void DeconstractionAssingment(int[] items)
        {
            {
                int x, y;

                // 既存の変数を使って分解
                (x, y) = Tally(items);
            }

            {
                var x = 1.0;
                var y = 5.0;

                for (int i = 0; i < 3; i++)
                {
                    (x, y) = ((x + y) / 2, Math.Sqrt(x * y));
                }

                Console.WriteLine($"{x}, {y}");
            }
        }

        private static void DeconstractionAssingment()
        {
            var a = new[] { 1, 2 };

            // 配列アクセス
            var b = new int[a.Length];
            (b[1], b[0]) = (a[0], a[1]);

            // 参照戻り値
            var c = new int[a.Length];
            (Mod(c, 5), Mod(c, 8)) = (a[0], a[1]);

            Console.WriteLine(string.Join(", ", b));
            Console.WriteLine(string.Join(", ", c));
        }

        static ref T Mod<T>(T[] array, int index) => ref array[index % array.Length];

        private static void DeconstractionDeclaration(int[] items)
        {
            {
                // count, sum を宣言しつつ、タプルを分解
                (int count, int sum) = Tally(items);

                // ↓こう書くとタプル型の変数の宣言
                // (int count, int sum) t = Tally(items);
            }

            {
                // 型推論で count, sum を宣言しつつ、タプルを分解
                (var count, var sum) = Tally(items);

                // ↓タプルだと var は使えない。これはコンパイル エラー
                // (var count, var sum) t = Tally(items);
            }

            {
                // 部分的に var を使う
                (var count, long sum) = Tally(items);
            }

            {
                // var + (変数リスト)でタプルを分解
                var (count, sum) = Tally(items);
            }
        }

        private static void ManuallyDeconstructed()
        {
            // この3行に相当する構文がほしい
            var x = Tally(new[] { 1, 2, 3, 4, 5 });
            var count = x.count;
            var sum = x.sum;
            // 以後、もう x は使わない

            Console.WriteLine(count);
            Console.WriteLine(sum);
        }

        private static void MeaninglessVariable()
        {
            var x = Tally(new[] { 1, 2, 3, 4, 5 });
            Console.WriteLine(x.count);
            Console.WriteLine(x.sum);
        }

        static (int count, int sum) Tally(IEnumerable<int> items)
        {
            var count = 0;
            var sum = 0;
            foreach (var x in items)
            {
                sum += x;
                count++;
            }

            return (count, sum);
        }

        static void Statements()
        {
            (int x, int y)[] array = new[] { (1, 2), (3, 4) };

            foreach (var (x, y) in array)
            {
                Console.WriteLine($"{x}, {y}");
            }

            for ((int i, int j) = (0, 0); i < 10; i++, j--)
            {
                Console.WriteLine($"{i}, {j}");
            }

            // ↓RTM までにはできるようになるはず？
            //var q =
            //    from (var x, vary) in array
            //    let (sum, product) = (x + y, x * y)
            //    select new { sum, product };
        }
    }
}
