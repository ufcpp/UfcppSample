using BenchmarkDotNet.Attributes;

#if DEBUG

var b = new GenericNewBenchmark();
Console.WriteLine(b.NonGenericNew());
Console.WriteLine(b.GenericNew());
Console.WriteLine(b.CreateInstance());
Console.WriteLine(b.StaticAbstractNew());

#else

using BenchmarkDotNet.Running;

BenchmarkRunner.Run<GenericNewBenchmark>();

#endif

/// <summary>
/// new() をジェネリックにやろうとするとどの程度遅くなるかを計測。
/// </summary>
/// <remarks>
/// 以下のようなテンプレコードで計測。
///
/// <code><![CDATA[
/// var sum = 0;
/// for (int i = 0; i < N; i++) sum += /* この分を差し替えて計測 */.Value;
/// return sum;
/// ]]></code>
/// </remarks>
[MemoryDiagnoser]
public class GenericNewBenchmark
{
    private const int N = 10000;
    private static T New<T>() where T : new() => new();
    private static T StaticAbstractNew<T>() where T : INew<T> => T.New();

    // 速い。というかインライン展開されて完全に消えてそうなベンチマーク結果。
    [Benchmark]
    public int NonGenericNew()
    {
        var sum = 0;
        for (int i = 0; i < N; i++) sum += new S().Value;
        return sum;
    }

    // 急に遅くなる。static abstract が速くできてこれが遅い理由もないんでそのうち最適化はされるかも…
    [Benchmark]
    public int GenericNew()
    {
        var sum = 0;
        for (int i = 0; i < N; i++) sum += New<S>().Value;
        return sum;
    }

    // 一番遅いけども、 New<S>() とそこまで差がない。
    // where T : new() => new() は結局 Activator.CreateInstance と同じ処理をしてるらしい。
    [Benchmark]
    public int CreateInstance()
    {
        var sum = 0;
        for (int i = 0; i < N; i++) sum += Activator.CreateInstance<S>().Value;
        return sum;
    }

    // Preview 機能を使えば…
    // 速い。 new() と同様インライン展開されて消えてそう。
    [Benchmark]
    public int StaticAbstractNew()
    {
        var sum = 0;
        for (int i = 0; i < N; i++) sum += StaticAbstractNew<S>().Value;
        return sum;
    }
}

public struct S : INew<S>
{
    public int Value;
    public S() => Value = 1;

    public static S New() => new();
}

// (C# 10 時点では Preview 必須)
public interface INew<T>
{
    public static abstract T New();
}
