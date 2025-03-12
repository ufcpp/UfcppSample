using BenchmarkDotNet.Attributes;
using ConsoleApp1;
using ConsoleApp1.Example;
using ConsoleApp1.Formatter;
using System.Buffers;

#if DEBUG

var b = new FormatterBenchmark();
b.Setup();

//for (int n = 0; n < 10000; n++)
{
    var x = b.ViaObject();
    var y = b.ViaUnsafeRef();
    AssertEqual(x, y);
}

static void AssertEqual(Line[] x, Line[] y)
{
    System.Diagnostics.Debug.Assert(x.Length == y.Length);
    for (int i = 0; i < x.Length; i++)
    {
        System.Diagnostics.Debug.Assert(x[i].Start.X == y[i].Start.X);
        System.Diagnostics.Debug.Assert(x[i].Start.Y == y[i].Start.Y);
        System.Diagnostics.Debug.Assert(x[i].End.X == y[i].End.X);
        System.Diagnostics.Debug.Assert(x[i].End.Y == y[i].End.Y);
    }
}

#else
BenchmarkDotNet.Running.BenchmarkRunner.Run<FormatterBenchmark>();
#endif

[MemoryDiagnoser]
public class FormatterBenchmark
{
    private const int N = 1000;

    private Line[]? _data;
    private ArrayBufferWriter<byte>? _writer;
    private FormatterProvider _provider;

    [GlobalSetup]
    public void Setup()
    {
        var data = _data = new Line[N];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new(new(3 * i, 5 * i), new(7 * i, 2 * i));
        }

        _writer = new(N * sizeof(int) * 4 + sizeof(int));

        var provider = _provider = new();
        provider.Add(Line.Formetter);
        provider.Add(Point.Formetter);
        _ = provider.GetFormatter(typeof(int));
    }

    [Benchmark]
    public Line[] ViaObject()
    {
        var writer = _writer;
        writer.Clear();
        var provider = _provider;
        var formatter = provider.GetFormatter(typeof(Line[]));

        formatter.Write(provider, writer, _data);
        var serialized = writer.WrittenMemory;

        var reader = new SequenceReader<byte>(new(serialized));
        var deserialized = (Line[])formatter.Read(provider, ref reader);

        return deserialized;
    }


    [Benchmark]
    public Line[] ViaUnsafeRef()
    {
        var writer = _writer;
        var provider = _provider;
        var formatter = provider.GetFormatter(typeof(Line[]));

        formatter.Write(provider, writer, UnsafeRef.Create(ref _data));
        var serialized = writer.WrittenMemory;

        var reader = new SequenceReader<byte>(new(serialized));
        Line[] deserialized = null!;
        formatter.Read(provider, ref reader, UnsafeRef.Create(ref deserialized));

        return deserialized;
    }
}