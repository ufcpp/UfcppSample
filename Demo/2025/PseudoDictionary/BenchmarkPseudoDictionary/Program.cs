using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PseudoDictionary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ValueList.Collections;

BenchmarkRunner.Run<PseudoDictionaryBenchmark>();

[MemoryDiagnoser]
public class PseudoDictionaryBenchmark
{
    private const string data = """
            a ab Abc x A a xy x abc ab A X y z xy b abc a X yZ zX z a b c x ab aBc
            """;

    private string[] _keys = null!;

    [GlobalSetup]
    public void Setup()
    {
        var keys = data.Split(' ');
        _keys = [.. keys, .. keys, .. keys, .. keys, .. keys, .. keys, .. keys, .. keys, .. keys];
    }

    [Benchmark]
    public void Dictionary()
    {
        var dic = new Dictionary<string, int>();

        foreach (var key in _keys)
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dic, key, out _)++;
        }
    }

    [Benchmark]
    public void PseudoDictionary()
    {
        var list = new List<(string key, int value)>();

        foreach (var key in _keys)
        {
            list.GetValueRefOrAddDefault(key)++;
        }
    }

    [InlineArray(32)]
    private struct Buffer<T>
    {
        private T _value;
    }

    [Benchmark]
    public void ValueList()
    {
        var buffer = new Buffer<(string key, int value)>();
        var list = new ValueListBuilder<(string key, int value)>(buffer);

        foreach (var key in _keys)
        {
            list.GetValueRefOrAddDefault(key)++;
        }
    }
}
