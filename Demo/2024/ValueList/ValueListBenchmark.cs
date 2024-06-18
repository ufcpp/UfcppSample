using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;
using ValueList.Collections;

namespace ValueList;

public class ValueListBenchmark
{
    private byte[] _bytes = null!;
    private short[] _shorts = null!;
    private int[] _ints = null!;
    private long[] _longs = null!;

    [GlobalSetup]
    public void Setup()
    {
        const int N = 100;
        var r = new Random();
        foreach (ref var x in (_bytes = new byte[N]).AsSpan()) x = (byte)r.Next();
        foreach (ref var x in (_shorts = new short[N]).AsSpan()) x = (short)r.Next();
        foreach (ref var x in (_ints = new int[N]).AsSpan()) x = (int)r.Next();
        foreach (ref var x in (_longs = new long[N]).AsSpan()) x = (long)r.Next();
    }

    [Benchmark]
    public ReadOnlyMemory<byte> ValueList()
    {
        var list = new ValueList<byte>();
        foreach (var x in _bytes) list.Add(x);
        foreach (var x in _shorts) MemoryMarshal.Write(list.AddSpan(sizeof(short)), x);
        foreach (var x in _ints) MemoryMarshal.Write(list.AddSpan(sizeof(int)), x);
        foreach (var x in _longs) MemoryMarshal.Write(list.AddSpan(sizeof(long)), x);
        return list.AsMemory();
    }

    [Benchmark]
    public ReadOnlyMemory<byte> PooledValueList()
    {
        using var list = new PooledValueList<byte>();
        foreach (var x in _bytes) list.Add(x);
        foreach (var x in _shorts) MemoryMarshal.Write(list.AddSpan(sizeof(short)), x);
        foreach (var x in _ints) MemoryMarshal.Write(list.AddSpan(sizeof(int)), x);
        foreach (var x in _longs) MemoryMarshal.Write(list.AddSpan(sizeof(long)), x);
        return list.AsMemory();
    }

    [Benchmark]
    public ReadOnlyMemory<byte> ValueListBuilder()
    {
        using var list = new ValueListBuilder<byte>(stackalloc byte[512]);
        foreach (var x in _bytes) list.Add(x);
        foreach (var x in _shorts) MemoryMarshal.Write(list.AddSpan(sizeof(short)), x);
        foreach (var x in _ints) MemoryMarshal.Write(list.AddSpan(sizeof(int)), x);
        foreach (var x in _longs) MemoryMarshal.Write(list.AddSpan(sizeof(long)), x);
        return list.AsMemory();
    }

    [Benchmark]
    public ReadOnlyMemory<byte> ReferenceImplementation()
    {
        using var list = new ReferenceImplementation<byte>(stackalloc byte[512]);
        foreach (var x in _bytes) list.Append(x);
        foreach (var x in _shorts) MemoryMarshal.Write(list.AppendSpan(sizeof(short)), x);
        foreach (var x in _ints) MemoryMarshal.Write(list.AppendSpan(sizeof(int)), x);
        foreach (var x in _longs) MemoryMarshal.Write(list.AppendSpan(sizeof(long)), x);
        return list.AsMemory();
    }
}
