using BenchmarkDotNet.Attributes;
using System;

/// <summary>
/// 文字列から Substring で部分文字列を抜き出して、
/// その部分文字列に対して foreach で文字を取り出すと言うのを繰り返すベンチマーク。
///
/// - <see cref="StandardString"/>
/// .NET 標準の string / <see cref="string.Substring(int, int)"/> を利用
///
/// - <see cref="AbstractString"/>
/// <see cref="global::AbstractString"/> を利用
///
/// - <see cref="SpanChar"/>
/// .NET 標準の string / <see cref="ReadOnlySpan{char}.Slice(int, int)"/> を利用
///
/// の3種類を対象にする。
///
/// 結果の例:
///          Method |      Mean |     Error |    StdDev |     Gen 0 | Allocated |
/// --------------- |----------:|----------:|----------:|----------:|----------:|
///  StandardString |  3.061 ms | 0.0206 ms | 0.0172 ms | 1242.1875 | 5218312 B |
///  AbstractString | 16.034 ms | 0.2279 ms | 0.2132 ms |  187.5000 |  802600 B |
///        SpanChar |  5.584 ms | 0.0472 ms | 0.0442 ms |         - |     560 B |
///
/// Allocated の列(メモリ確保量)を見ての通り、<see cref="string.Substring(int, int)"/> は1回1回文字列を new するので、ものすごいメモリを食う。
/// が、Gen 0 GC な限りものすごい高速。
/// (Gen 1/Gen 2 の GC 発生頻度が高い状況だともっと遅いはず。)
///
/// <see cref="global::AbstractString"/> はメモリ確保が6~7分の1に減ってるけども、実行速度的には5倍以上遅い。
/// 自作クラスなせいで遅い(.NET の string は JIT 時に条件が合えば範囲チェックを消したりとかいう最適化が掛かる)と言うのはもちろんあるものの。
/// そこを差し引いても <see cref="StandardString"/> より速くなることはないと思う。
/// virtual は最適化をかなり阻害する(インライン展開ができなくなったり)。
///
/// <see cref="ReadOnlySpan{T}"/> を使ったものは、メモリ確保量が激減する。
/// 今のところそれでもメモリ確保しまくりな <see cref="string.Substring(int, int)"/> の方が速い。
/// 前述の「JIT 時に範囲チェックを消す」最適化の差。
/// ただ、<see cref="Span{T}"/>/<see cref="ReadOnlySpan{T}"/>の最終目標は配列/stringと同程度の速度を達成することで、
/// Span に対しても「JIT 時に範囲チェックを消す」最適化を書ける作業をやってる最中らしく、将来的には速くなるはず。
/// </summary>
[MemoryDiagnoser]
public class SubstringBenchmark
{
    private static string Data = @"She speaks.
O, speak again, bright angel! For thou art
As glorious to this night, being o'er my head,
As is a winged messenger of heaven
Unto the white, upturned, wondering eyes
Of mortals that fall back to gaze on him
When he bestrides the lazy-puffing clouds
And sails upon the bosom of the air.
O Romeo, Romeo! Wherefore art thou Romeo?
Deny thy father and refuse thy name.
Or, if thou wilt not, be but sworn my love,
And I'll no longer be a Capulet.
Shall I hear more, or shall I speak at this?
'Tis but thy name that is my enemy.
Thou art thyself, though not a Montague.
What's Montague? It is nor hand, nor foot,
Nor arm, nor face, nor any other part
Belonging to a man. O, be some other name!
What's in a name? That which we call a rose
By any other word would smell as sweet.
So Romeo would, were he not Romeo called,
Retain that dear perfection which he owes
Without that title. Romeo, doff thy name,
And for that name, which is no part of thee
Take all myself.
";

    private const int Loops = 10000;
    private const int RandomSeed = 1;

    [Benchmark]
    public char[] StandardString()
    {
        var histogram = new char[128];

        var data = Data;
        var len = data.Length;
        var r = new Random(RandomSeed);

        for (int i = 0; i < Loops; i++)
        {
            var start = r.Next(len);
            var count = r.Next(len - start);

            var sub = data.Substring(start, count);

            foreach (var c in sub)
            {
                ref var h = ref histogram[c];
                ++h;
            }
        }

        return histogram;
    }

    [Benchmark]
    public char[] AbstractString()
    {
        var histogram = new char[128];

        var data = new FullString(Data); // 違うのはこの行だけ
        var len = data.Length;
        var r = new Random(RandomSeed);

        for (int i = 0; i < Loops; i++)
        {
            var start = r.Next(len);
            var count = r.Next(len - start);

            var sub = data.Substring(start, count);

            foreach (var c in sub)
            {
                ref var h = ref histogram[c];
                ++h;
            }
        }

        return histogram;
    }

    [Benchmark]
    public char[] SpanChar()
    {
        var histogram = new char[128];

        var data = Data;
        var len = data.Length;
        var r = new Random(RandomSeed);

        for (int i = 0; i < Loops; i++)
        {
            var start = r.Next(len);
            var count = r.Next(len - start);

            var sub = data.AsReadOnlySpan().Slice(start, count); // 違うのはこの行だけ

            foreach (var c in sub)
            {
                ref var h = ref histogram[c];
                ++h;
            }
        }

        return histogram;
    }
}
