using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace Inlining
{
    /// <summary>
    /// 結果の一例
    ///  Method |     Mean |     Error |    StdDev |
    /// ------- |---------:|----------:|----------:|
    ///      B0 | 3.261 ns | 0.0170 ns | 0.0150 ns | 手動展開
    ///      B1 | 3.072 ns | 0.0168 ns | 0.0149 ns |
    ///      B2 | 6.579 ns | 0.0550 ns | 0.0459 ns | NoInlining
    /// </summary>
    public class SimpleAdd
    {
        static int[] _a = new[] { 1, 2, 3, 4 };

        [Benchmark] public int B0() => F0(_a[0], _a[1], _a[2], _a[3]);
        [Benchmark] public int B1() => F1(_a[0], _a[1], _a[2], _a[3]);
        [Benchmark] public int B2() => F2(_a[0], _a[1], _a[2], _a[3]);

        [MethodImpl(MethodImplOptions.NoInlining)]
        int F0(int a, int b, int c, int d)
        {
            var e = a + b;
            var f = c + e;
            var g = d + f;
            return g;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int F1(int a, int b, int c, int d)
        {
            var e = Add1(a, b);
            var f = Add1(c, e);
            var g = Add1(d, f);
            return g;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int F2(int a, int b, int c, int d)
        {
            var e = Add2(a, b);
            var f = Add2(c, e);
            var g = Add2(d, f);
            return g;
        }

        // Aggressive を付けるまでもなくどう見てもインライン化掛かるレベル
        static int Add1(int a, int b) => a + b;

        // それをわざわざ阻害
        [MethodImpl(MethodImplOptions.NoInlining)]
        static int Add2(int a, int b) => a + b;
    }
}
