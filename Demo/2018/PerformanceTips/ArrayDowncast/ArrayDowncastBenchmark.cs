using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

public class ArrayDowncastBenchmark
{
    string[] _stringData = new string[] { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg" };
    object[] _objectData = new string[] { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg" };

    [Benchmark]
    public int MemberwiseCast()
    {
        // 要素ごとに (string)_data[i] なキャストが掛かる
        var sum = 0;
        foreach (string s in _objectData)
            sum += s.Length;
        return sum;
    }

    [Benchmark]
    public int ArrayCast()
    {
        // 最初に1回、配列自体のキャストが掛かる
        var data = (string[])_objectData;
        var sum = 0;
        foreach (var s in data)
            sum += s.Length;
        return sum;
    }

    public struct Wrap<T>
    {
        public T Value;
    }

    [Benchmark]
    public int UnsafeStructCast()
    {
        // 謎の最適化。
        // string 1個だけのフィールドを持つ構造体に無理やり変換して使う。
        // これで倍くらい速くなる。
        var data = Unsafe.As<object[], Wrap<string>[]>(ref _objectData);
        var sum = 0;
        foreach (var s in data)
            sum += s.Value.Length;
        return sum;
    }

    [Benchmark(Baseline = true)]
    public int Static()
    {
        // 比較対象として、元から string[] なものを列挙
        var sum = 0;
        foreach (var s in _stringData)
            sum += s.Length;
        return sum;
    }
}
