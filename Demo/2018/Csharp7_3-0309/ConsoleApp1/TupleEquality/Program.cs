using System;

namespace ConsoleApp1.TupleEquality
{
    class Program
    {
        static void M((int r, (int x, int y) c) t)
        {
            // こんな感じで、タプル同士の ==, != が書けるように。
            // これは、System.ValueTuple のユーザー定義演算子が呼ばれてるわけではなく、「メンバーごとの ==」に展開される。
            Console.WriteLine(t == (1, (2, 3)));

            // ↑こいつは、↓これと同じ意味。
            Console.WriteLine(t.r == 1 && t.c.x == 2 && t.c.y == 3);

            // != の方も。
            Console.WriteLine(t != (1, (2, 3)));
            Console.WriteLine(t.r != 1 || t.c.x != 2 || t.c.y != 3);

            // 何でこういうコンパイラーによる展開処理が必要かと言うと、暗黙的型変換を許すため。
            // 例えばこんなの。int と、double, decimal, long は暗黙的型変換が掛かるので、== で比較できる。
            // ValueTupe 越しだと、ValueTupe<int, ValueTupe<int, int>> と ValueTupe<double, ValueTupe<decimal, long>> になっちゃって、
            // ユーザー定義演算子だと比較できない。
            Console.WriteLine(t == (1.0, (2m, 3L)));
        }

        static void M((MyInt r, (MyInt x, MyInt y) c) t)
        {
            // MyInt 同士の == → MyBool になる。
            // その MyBool の && 扱い。
            // && の仕様から、!MyBool.false が呼ばれてる。
            Console.WriteLine(t == (1, (2, 3))); // r, c.x, が一致してたら、MyBool.false が3回表示される。

            // 同、!= の場合は || になって、|| の仕様から、!MyBool.true が3回。
            Console.WriteLine(t != (1, (2, 3))); // r, c.x, が一致してたら、MyBool.true が3回表示される。
        }

        static void Main()
        {
            M((1, (2, 3)));
            M((1, (2, 4)));

            M(((MyInt)1, (2, 3)));
            M(((MyInt)0, (1, 2)));
        }
    }

    // ==, != の結果が bool でなくて、ユーザー定義の bool 的な型(operator true, false が定義されてる)の場合のデモ用
    struct MyBool
    {
        public bool Value;
        public MyBool(bool value) => Value = value;

        // 何が呼ばれてるかがわかるように WriteLine を挟む
        public static bool operator true(MyBool x) { Console.WriteLine("MyBool.true"); return x.Value; }
        public static bool operator false(MyBool x) { Console.WriteLine("MyBool.false"); return !x.Value; }
        public static implicit operator MyBool(bool b) => new MyBool(b);
    }

    struct MyInt
    {
        public int Value;
        public MyInt(int value) => Value = value;
        public static MyBool operator ==(MyInt x, MyInt y) => x.Value == y.Value;
        public static MyBool operator !=(MyInt x, MyInt y) => x.Value != y.Value;
        public static implicit operator MyInt(int b) => new MyInt(b);
        public override bool Equals(object obj) => obj is MyInt x && Value == x.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
