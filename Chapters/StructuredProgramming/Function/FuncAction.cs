namespace Function.FuncAction
{
    using System;

    class Program
    {
        static void Main()
        {
            Action a1 = A1; // Func<void> とは書けない
            Action<int> a2 = A2;
            Func<int> f1 = F1; // Action と Func が別
            Func<int, int> f2 = F2;
        }

        static void A1() { } // 戻り値がないと、=> 記法も使えない
        static void A2(int x) { }
        static int F1() => 0;
        static int F2(int x) => x;
    }
}

namespace Function.UnitFunc
{
    using System;

    // 空っぽの型を1個用意
    struct Unit { }

    class Program
    {
        static void Main()
        {
            // void の代わりに Unit を使うことで、全部 Func に統一
            Func<Unit> a1 = A1;
            Func<int, Unit> a2 = A2;
            Func<int> f1 = F1;
            Func<int, int> f2 = F2;
        }

        static Unit A1() => default(Unit); // 空っぽの値を返しておく
        static Unit A2(int x) => default(Unit);
        static int F1() => 0;
        static int F2(int x) => x;
    }
}
