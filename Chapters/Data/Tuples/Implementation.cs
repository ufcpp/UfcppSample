namespace Tuples.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            LocalTuple();
            LocalTupleImpl();
            //Dynamic();
            LongTuple();
            LongTupleImple();
        }

        private static void LocalTuple()
        {
            var t = (x: 3, y: 5);
            var p = t.x * t.y;
            var (x, y) = t;
            Console.WriteLine($"{x} × {y} = {p}");
        }

        private static void LocalTupleImpl()
        {
            var t = new ValueTuple<int, int>(3, 5); // (x: 3, y: 5)
            var p = t.Item1 * t.Item2; // t.x * t.y
            var x = t.Item1;
            var y = t.Item2;
            Console.WriteLine($"{x} × {y} = {p}");
        }

        private static void Dynamic()
        {
            // 匿名型は名前が残る
            var a = new { x = 3, y = 5 };
            var s1 = Sum(a); // 大丈夫
            Console.WriteLine(s1);

            // タプル型は名前を紛失する
            var t = (x: 3, y: 5);
            var s2 = Sum(t); // x, yという名前が実行時になくてエラーに
            Console.WriteLine(s2);
        }

        private static dynamic Sum(dynamic d) => d.x + d.y;

        public (int x, int y) F((int a, int b) t) => (t.a + t.b, t.a - t.b);

#if false
        [return: TupleElementNames(new[] { "x", "y" })]
        public ValueTuple<int, int> F([TupleElementNames(new[] { "a", "b" })] ValueTuple<int, int> t)
            => new ValueTuple<int, int>(t.Item1 + t.Item2, t.Item1 - t.Item2);
#endif

        private static void LongTuple()
        {
            var t = (1, 2, 3, 4, 5, 6, 7, 8, 9);
            Console.WriteLine(t.Item9);
        }

        private static void LongTupleImple()
        {
            var t = new ValueTuple<int, int, int, int, int, int, int, ValueTuple<int, int>>(
                1, 2, 3, 4, 5, 6, 7, new ValueTuple<int, int>(8, 9));
            Console.WriteLine(t.Rest.Item2);
        }

        private static void Onetuple()
        {
            var noneple = new ValueTuple();
            var oneple = new ValueTuple<int>(1);

            // メンバー2個以上はタプル構文を使える
            var twople = (1, 2); // new ValueTuple<int, int>(1, 2);
            var threeple = (1, 2, 3); // new ValueTuple<int, int, int>(1, 2, 3);
        }

#if true
        // タプルでは0、1は書けない
        async Task F0() { }
        async Task<int> F1() => 1;
        async Task<(int x1, int x2)> F2() => (1, 2);
        async Task<(int x1, int x2, int x3)> F3() => (1, 2, 3);
#else
        // こう書けると統一性があってきれい(C# 7では書けない)
        async Task<()> F0() { }
        async Task<(int x1)> F1() => (1);
        async Task<(int x1, int x2)> F2() => (1, 2);
        async Task<(int x1, int x2, int x3)> F3() => (1, 2, 3);
#endif
    }
}
