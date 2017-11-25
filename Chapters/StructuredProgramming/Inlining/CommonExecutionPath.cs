using BenchmarkDotNet.Attributes;
using System;

namespace Inlining
{
    /// <summary>
    /// 結果の一例
    ///     Method |     Mean |    Error |   StdDev |
    /// ---------- |---------:|---------:|---------:|
    ///     Normal | 341.6 ns | 3.659 ns | 3.244 ns |
    ///  Optimized | 321.7 ns | 2.163 ns | 2.024 ns |
    ///
    /// あくまで「ほとんどの場合、Length == 1 または 2」という前提が当たっている場合に限ってなものの、何% か高速化されてる。
    /// </summary>
    public class CommonExecutionPath
    {
        static int[] _one = new[] { 1 };
        static int[] _two = new[] { 1, 2 };
        static int[] _long = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        [Benchmark]
        public void Normal()
        {
            // 長いの : 短いの = 1 : 25 の割合で、大半が短い配列に対する呼び出し
            for (int i = 0; i < 5; i++) Sum(_long);
            for (int i = 0; i < 25; i++) Sum(_two);
            for (int i = 0; i < 100; i++) Sum(_one);
        }

        [Benchmark]
        public void Optimized()
        {
            for (int i = 0; i < 5; i++) OptimizedSum(_long);
            for (int i = 0; i < 25; i++) OptimizedSum(_two);
            for (int i = 0; i < 100; i++) OptimizedSum(_one);
        }

        static int Sum(int[] a)
        {
            // ほとんどの場合、Length == 1 または 2 のところを通るという想定
            if (a.Length == 1) return a[0];
            else if (a.Length == 2) return a[0] + a[1];
            else if (a.Length >= 3)
            {
                // 反復がインライン化を阻害
                var sum = 0;
                foreach (var x in a)
                {
                    sum += x;
                }
                return sum;
            }

            // 例外がインライン化を阻害
            throw new IndexOutOfRangeException();
        }

        static int OptimizedSum(int[] a)
        {
            // ほとんどの場合、Length == 1 または 2 のところを通るという想定
            if (a.Length == 1) return a[0];
            else if (a.Length == 2) return a[0] + a[1];
            else if (a.Length >= 3) return LongSum(a);
            ThrowIndexOutOfRange();
            return 0;
        }

        // インライン化を阻害しているものを外に追い出す
        private static int LongSum(int[] a)
        {
            var sum = 0;
            foreach (var x in a)
            {
                sum += x;
            }
            return sum;
        }
        private static void ThrowIndexOutOfRange() => throw new IndexOutOfRangeException();
    }
}
