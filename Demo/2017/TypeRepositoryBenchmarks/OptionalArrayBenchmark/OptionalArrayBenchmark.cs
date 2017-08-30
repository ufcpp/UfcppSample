using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

/// <summary>
/// <see cref="Nullable{T}"/>とか<see cref="Optional{T}"/>みたいな型は、(bool, T) のペアなわけだけど、
/// アラインメントのためにたかが bool 1個のために数バイト(例えば T が参照型だった場合、T が8バイト、それに合せるために bool も8バイト)使っちゃう。
/// それを配列で持つとなるとちょっと無駄が多すぎる。
///
/// なので、<see cref="OptionalArray{T}"/>みたいな型を用意。
/// ただの T の配列 + bool を1か所で管理。
/// で、<see cref="Optional{T}"/>の配列と、その<see cref="OptionalArray{T}"/>の列挙の速度比較をしたい。
///
///  Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
/// ------- |---------:|----------:|----------:|-------:|----------:|
///       A | 2.555 us | 0.0172 us | 0.0144 us | 0.2518 |    1072 B |
///       B | 2.605 us | 0.0126 us | 0.0118 us | 0.1602 |     688 B |
///
/// 思ったよりも <see cref="OptionalArray{T}"/> のペナルティなさそう？
/// ヒープ確保量は想定通り、ほぼ半減。
/// 
/// ちなみに、<see cref="BitArray64"/>をクラスに変更
///       B | 2.916 us | 0.0102 us | 0.0091 us | 0.1678 |     712 B |
///
/// <see cref="BitArray64"/>を<see cref="System.Collections.BitArray"/>に変更
///  Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
///       B | 3.306 us | 0.0215 us | 0.0201 us | 0.1793 |     760 B |
/// ちょっとコスト高かなぁ…
///
/// それでも<see cref="OptionalArray{T}"/>のメリットは、配列の共変性をそのまま享受できる点。
/// <see cref="Optional{T}"/>の配列を作ると無理。
/// </summary>
[MemoryDiagnoser]
public class OptionalArrayBenchmark
{
    const int Seed = 12345678;
    const int Length = 48;

    [Benchmark]
    public double A()
    {
        var r = new Random();
        var array = new Optional<double>[Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = r.NextDouble() < 0.1 ? Optional<double>.None : new Optional<double>(r.NextDouble());
        }

        var sum = 0.0;

        for (int i = 0; i < array.Length; i++)
        {
            var x = array[i];
            if (x.HasValue) sum += x.GetValueOrDefault();
        }
        foreach (var x in array)
        {
            if (x.HasValue) sum += x.GetValueOrDefault();
        }
        return sum;
    }

    [Benchmark]
    public double B()
    {
        var r = new Random();
        var array = new OptionalArray<double>(Length);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = r.NextDouble() < 0.1 ? Optional<double>.None : new Optional<double>(r.NextDouble());
        }

        var sum = 0.0;

        for (int i = 0; i < array.Length; i++)
        {
            var x = array[i];
            if (x.HasValue) sum += x.GetValueOrDefault();
        }
        foreach (var x in array)
        {
            if (x.HasValue) sum += x.GetValueOrDefault();
        }
        return sum;
    }
}