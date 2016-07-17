namespace LocalFunctions.CaptureOptimization
{
    using System;

    class Program
    {
        static void M1(int m, int n)
        {
            // 最適化できる状況: ローカル関数を直接呼出し
            int f(int x, int y) => m * x + n * y;
            var r = f(3, 4);
        }

        static void M2(int m, int n)
        {
            // できない状況1: デリゲート越しに使っている
            int f(int x, int y) => m * x + n * y;
            Func<int, int, int> func = f;
            var r2 = func(3, 4);
        }

        static void M3(int m, int n)
        {
            // できない状況2: 匿名関数を使っている
            Func<int, int, int> f3 = (x, y) => m * x + n * y;
            var r3 = f3(3, 4);
        }
    }

    class CompilationResult
    {
        struct State
        {
            public int m;
            public int n;
        }

        static int Anonymous(int x, int y, ref State state)
        {
            return state.m * x + state.n * y;
        }

        static void M1(int m, int n)
        {
            // 最適化できる状況: ローカル関数を直接呼出し
            var state = new State { m = m, n = n };
            var r = Anonymous(3, 4, ref state);
        }
    }
}
