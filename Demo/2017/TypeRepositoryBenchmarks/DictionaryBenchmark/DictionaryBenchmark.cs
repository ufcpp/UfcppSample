using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// いくつか前提が絞られてる状況下で、文字列キーの辞書の性能上げれないかなぁという検討。
///
/// 前提:
/// ぶっちゃけ、クラス → Type とかプロパティ → PropertyInfo 的なものの辞書に使いたい。
/// - 文字列長はそんなに長くない
///   (もしかしたら、ASCII 文字前提にしてもいいかも。少なくとも99%くらいASCII文字)
/// - キーの数もそこまで多くない
///   (型数は千個くらい。プロパティ数は大半が10以下、多くてせいぜい数十。)
/// - 増減しない
///
/// 場合によっては<see cref="System.Linq.Expressions.LambdaExpression.Compile"/>とかで switch とかをコード生成するのが最速なのでそれも比較に並べたいけど、
/// ベンチマーク上、最初の Compile に掛かる時間を避けるのをどうやればいいかで悩み中。
///
///    Method |      Mean |     Error |    StdDev |
/// --------- |----------:|----------:|----------:|
///  CharNode | 11.689 ms | 0.0681 ms | 0.0568 ms |
///  LongNode |  9.125 ms | 0.0513 ms | 0.0480 ms |
///       Sys |  2.396 ms | 0.0707 ms | 0.0814 ms |
///     Fixed |  2.266 ms | 0.0151 ms | 0.0141 ms |
/// </summary>
public class DictionaryBenchmark
{
    string[] allKeys;

    [GlobalSetup]
    public void Setup()
    {
        allKeys = TestData.TypeNames.SelectMany(x => x.propertyNames).Distinct().ToArray();
    }

    [Benchmark]
    public void CharNode()
    {
        foreach (var (t, p) in TestData.TypeNames.Take(1))
        {
            // 追加は1回限り
            var items = p.Select(x => new KeyValuePair<string, string>(x, x));
            var d = new CharNode<string>();
            foreach (var (key, value) in items) d.Add(key, value);
            Bench(items, allKeys.Except(p), d);
        }
    }

    [Benchmark]
    public void LongNode()
    {
        foreach (var (t, p) in TestData.TypeNames.Take(1))
        {
            var items = p.Select(x => new KeyValuePair<string, string>(x, x));
            var d = new LongNode<string>();
            foreach (var (key, value) in items) d.Add(key, value);
            Bench(items, allKeys.Except(p), d);
        }
    }

    [Benchmark]
    public void Sys()
    {
        foreach (var (t, p) in TestData.TypeNames.Take(1))
        {
            var items = p.Select(x => new KeyValuePair<string, string>(x, x));
            var d = new Dictionary<string, string>();
            foreach (var (key, value) in items) d.Add(key, value);
            Bench(items, allKeys.Except(p), d);
        }
    }

    struct StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y) => x == y;
        public int GetHashCode(string obj) => obj.GetHashCode();
    }

    [Benchmark]
    public void Fixed()
    {
        foreach (var (t, p) in TestData.TypeNames.Take(1))
        {
            var items = p.Select(x => new KeyValuePair<string, string>(x, x));
            var d = new FixedDictionary<string, string, StringComparer>(items);
            Bench(items, allKeys.Except(p), d);
        }
    }

    private static void Bench<T, TDictionary>(IEnumerable<KeyValuePair<string, T>> items, IEnumerable<string> otherKeys, TDictionary dic)
        where TDictionary : IDictionary<string, T>
    {
        // 読む方が圧倒的に多いという想定

        // 特に、ちゃんとあるはずのキーで読むことが大半
        for (int i = 0; i < 200; i++)
        {
            foreach (var (key, value) in items)
            {
                var v = dic[key];
                if (!EqualityComparer<T>.Default.Equals(value, v)) throw new Exception();
                if (!dic.TryGetValue(key, out _)) throw new Exception();
            }
        }

        // ないはずのキーでの検索(失敗前提)は↑の20分の1にする
        for (int i = 0; i < 10; i++)
        {
            foreach (var key in otherKeys)
            {
                if (dic.TryGetValue(key, out _)) throw new Exception();
            }
        }
    }
}
