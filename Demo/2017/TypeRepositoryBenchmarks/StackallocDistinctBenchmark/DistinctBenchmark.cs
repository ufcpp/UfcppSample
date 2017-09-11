using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

/// <summary>
/// いろんなベンチマーク取ってて至った境地が「stackalloc 強すぎ」だったので、
/// 手段と目的を逆転させて stackalloc で何かできないか考えた結果。
///
/// サイズが最初からわかってる配列に対する Distinct を高速化できるんじゃないかと思ってやってみた。
/// 原理は、stackalloc でスタック上にハッシュテーブルを構築して、それで重複チェック。
/// (削除を考えなくていいハッシュテーブルは割とシンプルに書ける。)
///
/// stackalloc の性質上、
/// - yield や await を挟めない(あくまで一括処理できるデータ列のみが対象)
/// - あんまり大きな配列に対しては使えない(スタックオーバーフローを起こす)
///
/// 結果の一例:
///
///  Method |      Mean |     Error |    StdDev |   Gen 0 | Allocated |
/// ------- |----------:|----------:|----------:|--------:|----------:|
///   Linq0 | 33.216 us | 0.4380 us | 0.4097 us | 16.6626 |   70032 B |
///   Linq1 | 32.803 us | 0.3541 us | 0.3312 us |  8.4839 |   35664 B |
///   Linq2 | 18.743 us | 0.0964 us | 0.0902 us |  1.0986 |    4736 B |
///   Linq3 | 17.365 us | 0.0765 us | 0.0678 us |  0.1526 |     672 B |
///  Array0 |  4.652 us | 0.0356 us | 0.0333 us |  4.8981 |   20576 B |
///  Array1 |  4.448 us | 0.0515 us | 0.0482 us |  4.1656 |   17504 B |
///  Array2 |  3.285 us | 0.0119 us | 0.0099 us |  3.1395 |   13184 B |
///  Array3 |  3.098 us | 0.0080 us | 0.0075 us |  2.9640 |   12464 B |
///  Stack0 |  4.202 us | 0.0815 us | 0.0722 us |  1.9608 |    8240 B |
///  Stack1 |  3.864 us | 0.0273 us | 0.0255 us |  1.2360 |    5200 B |
///  Stack2 |  2.957 us | 0.0139 us | 0.0116 us |  0.1984 |     848 B |
///  Stack3 |  2.716 us | 0.0133 us | 0.0104 us |  0.0229 |     128 B |
///
/// LINQ と自作のを比較してる。
/// - Linq から始まるやつが <see cref="Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>を使ったもの。
/// - Array から始まるやつは、長さが最初から分かってる前提で最適化実装したもの。ただし、一時バッファは配列(普通に new)。
/// - Stack から始まるやつが今回の主役の stackalloc版。new が stackalloc に変わった以外は Array と同じ。
///
/// Distinct の対象にするデータは4種類あって、
/// - 0 … 全部違う値
/// - 1 … 0～999までの範囲(1000種)でランダム
/// - 2 … 0～90までの範囲(100種)でランダム
/// - 3 … 0～9までの範囲(10種)でランダム
/// (種類が少ないほどハッシュ値の衝突が少なくて速い)
/// で、いずれも1024要素の配列。
/// </summary>
[MemoryDiagnoser]
public class DistinctBenchmark
{
    const int N = 1024;
    int[] data0;
    int[] data1;
    int[] data2;
    int[] data3;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random();

        data0 = new int[N];
        for (int i = 0; i < data0.Length; i++) data0[i] = i;

        data1 = new int[N];
        for (int i = 0; i < data1.Length; i++) data1[i] = r.Next(0, 1000);

        data2 = new int[N];
        for (int i = 0; i < data2.Length; i++) data2[i] = r.Next(0, 100);

        data3 = new int[N];
        for (int i = 0; i < data3.Length; i++) data3[i] = r.Next(0, 10);
    }

    [Benchmark] public int[] Linq0() => data0.Distinct().ToArray();
    [Benchmark] public int[] Linq1() => data1.Distinct().ToArray();
    [Benchmark] public int[] Linq2() => data2.Distinct().ToArray();
    [Benchmark] public int[] Linq3() => data3.Distinct().ToArray();
    [Benchmark] public int[] Array0() => ArrayHashSet.Distinct<int, IntComp>(data0);
    [Benchmark] public int[] Array1() => ArrayHashSet.Distinct<int, IntComp>(data1);
    [Benchmark] public int[] Array2() => ArrayHashSet.Distinct<int, IntComp>(data2);
    [Benchmark] public int[] Array3() => ArrayHashSet.Distinct<int, IntComp>(data3);
    [Benchmark] public int[] Stack0() => StackHashSet.Distinct<int, IntComp>(data0);
    [Benchmark] public int[] Stack1() => StackHashSet.Distinct<int, IntComp>(data1);
    [Benchmark] public int[] Stack2() => StackHashSet.Distinct<int, IntComp>(data2);
    [Benchmark] public int[] Stack3() => StackHashSet.Distinct<int, IntComp>(data3);
}
