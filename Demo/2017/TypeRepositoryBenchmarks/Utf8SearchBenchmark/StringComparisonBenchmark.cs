using BenchmarkDotNet.Attributes;
using System;
using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// おまけ。
///
/// <see cref="Utf8SearchBenchmark"/>を作るためにC# boyer mooreとかでググる
/// ↓
/// string.IndexOf の性能に関するStackOverlfow投稿が目に付く
/// ↓
/// Ordinal付けるかどうかで性能差すごいらしい？
/// ↓
/// 30倍違った…(今ここ)
/// 
/// こんなの、常に Ordinal 付けるしかないじゃない…
/// (デフォルト動作が CurrentCulture なのつらい。)
///
///                 Method |        Mean |      Error |     StdDev | Allocated |
/// ---------------------- |------------:|-----------:|-----------:|----------:|
///  CurrentCultureIndexOf | 16,411.8 us | 173.195 us | 135.219 us |       0 B |
///         OrdinalIndexOf |    552.2 us |   2.131 us |   1.889 us |       0 B |
///
/// ちなみに、<see cref="StringComparison.Ordinal"/>以外での<see cref="string.IndexOf(string)"/>は、前方から1文字1文字調べていくタイプのアルゴリズムっぽい。
/// ↑いわゆる O(N * M) のやつ(N が検索対象の、M が検索したいパターンの長さ)。
/// <see cref="StringComparison.Ordinal"/>の時だけネイティブ実装に飛ばされて、おそらくその中で O(N / M) なアルゴリズムが使われてる。
/// </summary>
[MemoryDiagnoser]
public class StringComparisonBenchmark
{
    string text;
    string[] patterns;

    [GlobalSetup]
    public void Setup()
    {
        text = LoadDataAsync().GetAwaiter().GetResult();

        async Task<string> LoadDataAsync()
        {
            var c = new HttpClient();
            var res = await c.GetAsync("http://ufcpp.net/study/csharp/cheatsheet/ap_ver7/");
            return await res.Content.ReadAsStringAsync();
        }

        patterns = new[]
        {
            "C#",
            "throw",
            "new",
            "演算子",
            "タプル",
            "なさそうなもじれつ",
            "一番重要視しているのは生産性の高さで、書きやすさ、読みやすさなどが一番大事です。",
            "<span",
            " ",
        };
    }

    [Benchmark]
    public int CurrentCultureIndexOf()
    {
        var sum = 0;
        foreach (var p in patterns)
            sum += Count(text, p, StringComparison.CurrentCulture);
        return sum;
    }

    [Benchmark]
    public int OrdinalIndexOf()
    {
        var sum = 0;
        foreach (var p in patterns)
            sum += Count(text, p, StringComparison.Ordinal);
        return sum;
    }

    private static int Count(string text, string pattern, StringComparison comp)
    {
        var count = 0;
        var i = 0;
        while (true)
        {
            i = text.IndexOf(pattern, i, comp);
            if (i < 0) return count;
            count++;
            i++;
        }
    }
}
