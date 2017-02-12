using System;

namespace ConsoleApp1._01_Tuples
{
    class Implementation
    {
        // 元
        public static void X1()
        {
            var (a, b) = Y1(1, 2);
            (a, b) = (b, a);

            var longTuple = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Console.WriteLine(longTuple.Item10);
        }

        public static (int a, int b) Y1(int x, int y) => (x + y, x - y);

        // ↓

        public static void X2()
        {
            //var (a, b) = Y1(1, 2);
            // 普通に2行の変数宣言に展開
            var t = Y2(1, 2);
            var a = t.Item1;
            var b = t.Item2;

            //(a, b) = (b, a);
            // 単純にタプル作成 → 分解 
            var temp = new ValueTuple<int, int>(b, a);
            a = temp.Item1;
            b = temp.Item2;

            //var longTuple = (1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            // ValueTuple<> は8引数までしかない
            // それを超えた分は再帰的に展開
            var longTuple = new ValueTuple<int, int, int, int, int, int, int, ValueTuple<int, int, int>>(1, 2, 3, 4, 5, 6, 7, new ValueTuple<int, int, int>(8, 9, 10));

            //Console.WriteLine(longTuple.Item10);
            // Item10っていうメンバーも本来は持ってない。再帰的に展開
            Console.WriteLine(longTuple.Rest.Item3);
        }

        // この属性、C# 7だと「タプル構文を使え」というエラーになる
        //[TupleElementNames(new[] { "a", "b" })]
        public static ValueTuple<int, int> Y2(int x, int y) => ValueTuple.Create(x + y, x - y);
    }
}
