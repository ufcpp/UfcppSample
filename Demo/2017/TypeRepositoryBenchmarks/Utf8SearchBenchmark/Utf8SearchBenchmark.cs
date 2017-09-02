using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// だいたい UTF8 → string への変換(と、string 自体のヒープアロケーション)がつらいことはわかってる
/// ↓
/// しょうがないから UTF8 のままでいろいろと処理
/// ↓
/// 標準ライブラリに頼れないし、ネットで検索しても出てるコードは大体 string 前提
/// ↓
/// SequenceEqual、GetHashCode、IndexOf 辺りを自作(今IndexOf)
///
/// 雑に数時間で書いた割には、標準ライブラリの<see cref="string.LastIndexOf(string, StringComparison)"/>の3倍程度の遅さで済んでるので及第点かなぁ。
///
///             Method |       Mean |    Error |   StdDev |
/// ------------------ |-----------:|---------:|---------:|
///     OrdinalIndexOf |   553.8 us | 6.796 us | 6.024 us | ← 標準の
///  BoyerMooreIndexOf | 1,600.2 us | 4.034 us | 3.773 us | ← 自作の
/// </summary>
public class Utf8SearchBenchmark
{
    (string utf16, byte[] utf8) text;
    (string utf16, byte[] utf8)[] patterns;

    [GlobalSetup]
    public void Setup()
    {
        async Task<byte[]> LoadDataAsync()
        {
            var c = new HttpClient();
            var res = await c.GetAsync("http://ufcpp.net/study/csharp/cheatsheet/ap_ver7/");
            return await res.Content.ReadAsByteArrayAsync();
        }

        text.utf8 = LoadDataAsync().GetAwaiter().GetResult();
        text.utf16 = Encoding.UTF8.GetString(text.utf8);

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
        }
        .Select(s => (s, Encoding.UTF8.GetBytes(s)))
        .ToArray();
    }

    [Benchmark]
    public int OrdinalIndexOf()
    {
        var sum = 0;
        foreach (var (utf16, _) in patterns)
            sum += Count(text.utf16, utf16, StringComparison.Ordinal);
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

    [Benchmark]
    public int BoyerMooreIndexOf()
    {
        var sum = 0;
        foreach (var (_, utf8) in patterns)
            sum += Count(text.utf8, utf8);
        return sum;
    }

    private static int Count(byte[] text, byte[] pattern)
    {
        var count = 0;
        Span<byte> s = text;
        while (true)
        {
            var i = BoyerMoore.IndexOf(s, pattern);
            if (i == s.Length) return count;
            count++;
            s = s.Slice(i + 1);
        }
    }
}
