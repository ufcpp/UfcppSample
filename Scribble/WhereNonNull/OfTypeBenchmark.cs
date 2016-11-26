using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Linq;

namespace WhereNonNull
{
    /* 参考までに、1回実行してみた結果。
                       Method |           Mean |      StdDev |         Median |
----------------------------- |--------------- |------------ |--------------- |
            OfTypeEnumeration | 19,039.9305 ns | 287.1109 ns | 19,050.3596 ns |
        SelectManyEnumeration |  9,994.0711 ns | 121.4594 ns |  9,962.7680 ns |
       SelectManyEnumeration2 |  9,618.4544 ns | 168.9270 ns |  9,555.7751 ns |
       SelectWhereEnumeration |  3,507.6555 ns |  56.6892 ns |  3,478.8764 ns |
 DedicatedIteratorEnumeration |  2,621.3351 ns |  39.6181 ns |  2,608.8703 ns |
   DedicatedStructEnumeration |  1,822.5345 ns |  22.6200 ns |  1,819.7174 ns |
            NoLinqEnumeration |    347.4957 ns |   2.7167 ns |    347.7183 ns |
    */
    [RyuJitX64Job]
    public class OfTypeBenchmark
    {
        int?[] data;

        [Setup]
        public void Setup()
        {
            data = new int?[] { 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, 1, null, 2, null, 3, null, 4, null, 5, null, };
        }

        /// <summary>
        /// <see cref="Enumerable.OfType{TResult}(IEnumerable)"/>で T? → T 変換。
        /// 案外遅い。
        /// 本来、nullのフィルタリングに使うもんじゃない。
        /// 19,039.9305 ns
        /// </summary>
        [Benchmark]
        public int OfTypeEnumeration()
        {
            var sum = 0;
            foreach (var x in data.OfType<int>())
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// <see cref="Nullable{T}"/> → <see cref="IEnumerable{T}"/> 変換コードを書いて、
        /// それを<see cref="Enumerable.SelectMany{TSource, TResult}(IEnumerable{TSource}, Func{TSource, IEnumerable{TResult}})"/>と組み合わせたもの。
        /// まあ、無駄な処理してる。
        /// それでもOfTypeよりはマシ。
        /// 9,994.0711 ns
        /// </summary>
        [Benchmark]
        public int SelectManyEnumeration()
        {
            var sum = 0;
            foreach (var x in data.SelectMany(x => x.AsEnumerable()))
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// SelectManyな実装で手を抜いて、new T[]{} しちゃうパターン。
        /// どうせLINQを介する時点で<see cref="NullableEnumerable{T}"/>構造体がボックス化しちゃうんで、あんまり<see cref="SelectManyEnumeration"/>と大差ない。
        /// 9,618.4544 ns
        /// </summary>
        [Benchmark]
        public int SelectManyEnumeration2()
        {
            var sum = 0;
            foreach (var x in data.SelectMany(x => x.HasValue ? new[] { x.GetValueOrDefault() } : Array.Empty<int>()))
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// まあ、素直にSelectManyあきらめろってこった。
        /// <see cref="Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>でnullをはじく作り。
        /// 3,507.6555 ns
        /// </summary>
        [Benchmark]
        public int SelectWhereEnumeration()
        {
            var sum = 0;
            foreach (var x in data.Where(x => x.HasValue).Select(x => x.GetValueOrDefault()))
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// 非nullなものだけ取り出す専用の関数を書いたもの。
        /// 専用化するとさすがに速い。
        /// 2,621.3351 ns
        /// </summary>
        [Benchmark]
        public int DedicatedIteratorEnumeration()
        {
            var sum = 0;
            foreach (var x in data.NonNull1())
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// 非nullなものだけ取り出す専用の関数を書いたもの。
        /// イテレーターすら使うのをやめて、structでenumerable/enumeratorを実装したもの。
        /// 1,822.5345 ns
        /// </summary>
        [Benchmark]
        public int DedicatedStructEnumeration()
        {
            var sum = 0;
            foreach (var x in data.NonNull())
            {
                sum += x;
            }
            return sum;
        }

        /// <summary>
        /// まあ、メソッドチェーンしなければ最速なんだけど…
        /// というか、これが文字通り「桁違い」に早い…
        /// 347.4957 ns
        /// </summary>
        [Benchmark]
        public int NoLinqEnumeration()
        {
            var sum = 0;
            foreach (var x in data)
            {
                if (x.HasValue)
                    sum += x.GetValueOrDefault();
            }
            return sum;
        }
    }
}