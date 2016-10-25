namespace Tuples.ErroneousSamples
{
#if false
    using System;

    class Program
    {
        static void Main()
        {
            () noneple;     // ダメ
            (int x) oneple; // ダメ

            // タプル構文で書けるのは2つ以上だけ
            (int x, int y) twople; // OK

            // タプル構文でなければ、0-tuple, 1-tuple も作れる
            ValueTuple none;     // OK
            ValueTuple<int> one; // OK
        }
    }
#endif

#if false
    // using でエイリアスを付けることはできない
    using T = (int x, int y);

    class Program
    {
        static void Main()
        {
            // var t = new T(1, 2); みたいなのと同じノリでは書けない
            var t1 = new(int x, int y)(1, 2);
            var t2 = new(int x, int y) { x = 1, y = 2 };
        }

        static void M(object obj)
        {
            // is 演算子には使えない
            if(obj is (int x, int y))
            {
            }
        }
    }
#endif

#if false
    using System;

    class Program
    {
        static void Main()
        {
            var ticks = 100000;
            // C# 8?
            DateTime d = new(ticks); // 左辺から型推論して、new DateTime(ticks) が呼ばれる
        }

        static void M(object obj)
        {
            // C# 8?
            // is T 扱いじゃなくて、パターン マッチングで obj を x, y に分解
            if (obj is (int x, int y))
            {
                Console.WriteLine($"{x}, {y}");
            }
        }
    }
#endif
}
