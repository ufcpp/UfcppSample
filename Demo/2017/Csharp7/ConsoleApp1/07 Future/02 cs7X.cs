namespace ConsoleApp1._07_Future._7X
{
    // C# 7.X 予定リスト
    // 7.1, 7.2 よりは具体性がなく、7.3 になるか 7.4 になるか… みたいなあいまいな感じ。

#if false

    using System;
    using System.Collections.Generic;
    using ConsoleApp1._02_Patterns;

    // Pattern Matching
    static class NodeExtensions
    {
        // 02 Patterns で例に出したやつをちょこっと変更
        public static Node Reduce(this Node n)
        {
            // 一か所にコードが集まってることが利点になることもある。コード分量次第。ｚ
            switch (n)
            {
                case Add(Const x, Const y): return new Const(x.Value + y.Value);
                case Add(Const x, var y) when x.Value == 0: return y;
                case Add(var x, Const y) when y.Value == 0: return x;
                case Mul(Const x, Const y): return new Const(x.Value * y.Value);
                case Mul(Const x, var y) when x.Value == 0: return new Const(0);
                case Mul(var x, Const y) when y.Value == 0: return new Const(0);
                case Mul(Const x, var y) when x.Value == 1: return y;
                case Mul(var x, Const y) when y.Value == 1: return x;
                default: return n;
            }
        }
    }

    // Bestest Betterness
    // オーバーロード解決の際、「より一致度の高い方を選ぶ」というのを指して「betterness rule」ってのを決めてる
    // 「オーバーロード解決が賢くなったよ」ってのを指して「better betterness」って言ってたのが、
    // その次のバージョンで best betterness
    // さらに今、bestest betterness
    // とか言ってる。

    interface IA { }
    interface IB { }
    class X : IA, IB { }

    class StaticResolutionSample
    {
        // 呼び分けが難しそうなもの
        static void F(IA a) { }
        void F(IB b) { }

        // 比較用
        static void FA(IA a) { }
        void FB(IB b) { }

        void X()
        {
            Action<X> fa = FA; // OK
            Action<X> fb = FB; // OK

            // 現状、解決不能
            Action<X> a = F;      // this が付いてなかったら static 扱いで IA の方でいいのではないか
            Action<X> b = this.F; // this を付けたら IB の方
        }
    }

    class ConstraintResolutionSample
    {
        // 呼び分けが難しそうなもの
        static void F<T>(T x) where T : struct { }
        static void F<T>(IEnumerable<T> x) { }

        // 比較用
        static void FT<T>(T x) where T : struct { }
        static void FE<T>(IEnumerable<T> x) { }

        void X()
        {
            int[] array = null;

            FT(array); // NG。struct 制約に合わない
            FE(array); // OK

            // 現状、解決不能
            F(array); // F(T) の方は制約に合わないから候補から外して、F(IEnumerable<T>) の方を選べるべきではないか
        }
    }

    class ReturnResolutionSample
    {
        // 呼び分けが難しそうなもの
        static int F(IA x) => 0;
        static string F(IB x) => "";

        // 比較用
        static int FI(IA x) => 0;
        static string FS(IB x) => "";

        void X()
        {
            Func<X, int> fi = FI;    // OK。引数に関して反変性が効いてる
            Func<X, string> fs = FS; // OK。同上

            // 現状、解決不能
            Func<X, int> f1 = F;    // 戻り値の型も見て、int F(IA) を選べるべきではないか
            Func<X, string> f2 = F; // 同上、string F(IB)
        }
    }

#endif
}
