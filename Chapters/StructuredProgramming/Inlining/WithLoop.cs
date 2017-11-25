using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Inlining
{
    /// <summary>
    /// 結果の一例
    ///  Method |      Mean |     Error |    StdDev |
    /// ------- |----------:|----------:|----------:|
    ///      B0 |  6.955 ns | 0.0476 ns | 0.0344 ns | 手動展開
    ///      B1 | 10.258 ns | 0.1549 ns | 0.1373 ns |
    ///      B2 |  6.844 ns | 0.0431 ns | 0.0360 ns | AggressiveInlining
    ///      B3 | 10.219 ns | 0.1083 ns | 0.0846 ns |
    ///
    /// - インライン化の有無で倍近く速度差あり
    ///   - 配列が短いせい。配列が長くなると、関数呼び出しのコストは相対的に小さくなる
    /// - AggressiveInlining の時だけインライン化されてそう
    ///   - おそらく、「ループを含んでると通常はインライン化しない」仕様
    ///   - 関数の中身のサイズ(Sum の場合は28バイト)的には AggressiveInlining なしでもインライン化されうる範疇のはず
    /// </summary>
    public class WithLoop
    {
        static int[] _a = new[] { 1, 2, 3 };
        static int[] _b = new[] { -1, 4, -2 };

        [Benchmark] public int B0() => F0(_a, _b);
        [Benchmark] public int B1() => F1(_a, _b);
        [Benchmark] public int B2() => F2(_a, _b);
        [Benchmark] public int B3() => F3(_a, _b);

        static int F0(int[] a, int[] b)
        {
            var sumA = 0;
            foreach (var x in a)
            {
                sumA += x;
            }
            var sumB = 0;
            foreach (var x in b)
            {
                sumB += x;
            }

            return Math.Max(sumA, sumB);
        }

        static int F1(int[] a, int[] b)
        {
            var sumA = Sum(a);
            var sumB = Sum(b);

            return Math.Max(sumA, sumB);
        }

        static int F2(int[] a, int[] b)
        {
            var sumA = SumAgressive(a);
            var sumB = SumAgressive(b);

            return Math.Max(sumA, sumB);
        }

        static int F3(int[] a, int[] b)
        {
            var sumA = SumNo(a);
            var sumB = SumNo(b);

            return Math.Max(sumA, sumB);
        }

        static int Sum(int[] a)
        {
            var sum = 0;
            foreach (var x in a)
            {
                sum += x;
            }
            return sum;
        }

        // 積極的にインライン化してもらいたい
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int SumAgressive(int[] a)
        {
            var sum = 0;
            foreach (var x in a)
            {
                sum += x;
            }
            return sum;
        }

        // 全くインライン化させたくない
        [MethodImpl(MethodImplOptions.NoInlining)]
        static int SumNo(int[] a)
        {
            var sum = 0;
            foreach (var x in a)
            {
                sum += x;
            }
            return sum;
        }
    }
}
