using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace FastEnumeration
{
    /// <summary>
    /// 配列に対する和の計算を、<see cref="NormalEnumerator"/> と <see cref="FastEnumerator"/> を介するのでどのくらい違うかを試す。
    /// あと、具象型を渡す(non-virtual なのでインライン化を期待できる)と、インターフェイスを渡す(virtual なのでインライン化されない)でどのくらい差が付くかも試す。
    ///
    /// 実行例:
    ///            Method |     Mean |    Error |   StdDev |
    /// ----------------- |---------:|---------:|---------:|
    ///  NonVirtualNormal | 165.8 ns | 2.225 ns | 2.082 ns |
    ///    NonVirtualFast | 161.2 ns | 2.208 ns | 2.065 ns |
    ///     VirtualNormal | 449.6 ns | 2.595 ns | 2.300 ns |
    ///       VirtualFast | 271.3 ns | 2.338 ns | 2.072 ns |
    ///
    /// non-virtual な場合(具象型を直接渡す場合)はどの道インライン化で最適化がかかるのでそんなに差が出ない。
    ///
    /// 問題は virtual に呼んだ場合(インターフェイスを介して呼んだ)で、4割くらい差が出る。
    /// 今回の場合、MoveNext/Current/TryMoveNext の呼び出し以外にやってることがほとんどないので、virtual call の負担の差がはっきりと出る(たぶん上限)。
    /// あと、MoveNext/Current の場合、インデックスの範囲チェック(i &lt; length)とアクセス(data[i])がわかれることで、配列の最適化が効きにくくなってるはず。
    /// </summary>
    public class ForeachBenchmark
    {
        int[] _data;

        [GlobalSetup]
        public void Setup()
        {
            _data = Enumerable.Range(0, 100).ToArray();
        }

        [Benchmark]
        public int NonVirtualNormal() => NonVirtualSum(new NormalEnumerator(_data));

        [Benchmark]
        public int NonVirtualFast() => NonVirtualSum(new FastEnumerator(_data));

        [Benchmark]
        public int VirtualNormal() => VirtualSum(new NormalEnumerator(_data));

        [Benchmark]
        public int VirtualFast() => VirtualSum(new FastEnumerator(_data));

        [Benchmark]
        public int Adapter() => GenericSum(new Adapter<int>(new FastEnumerator(_data)));

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int NonVirtualSum(NormalEnumerator e)
        {
            var sum = 0;
            while (e.MoveNext())
            {
                var x = e.Current;
                sum += x;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int NonVirtualSum(FastEnumerator e)
        {
            var sum = 0;
            while (true)
            {
                var x = e.TryMoveNext(out var success);
                if (!success) break;
                sum += x;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int GenericSum<T>(T e)
            where T : IEnumerator<int>
        {
            var sum = 0;
            while (e.MoveNext())
            {
                var x = e.Current;
                sum += x;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int VirtualSum(IEnumerator<int> e)
        {
            var sum = 0;
            while (e.MoveNext())
            {
                var x = e.Current;
                sum += x;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static int VirtualSum(IFastEnumerator<int> e)
        {
            var sum = 0;
            while (true)
            {
                var x = e.TryMoveNext(out var success);
                if (!success) break;
                sum += x;
            }
            return sum;
        }
    }
}
