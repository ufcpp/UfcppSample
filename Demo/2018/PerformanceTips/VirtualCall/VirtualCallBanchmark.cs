using BenchmarkDotNet.Attributes;

interface IValue { int Value { get; } }
class Impl : IValue { public int Value => 0; }

public class VirtualCallBanchmark
{
    // インターフェイス越し
    IValue A { get; } = new Impl();

    // クラスを直公開
    Impl B { get; } = new Impl();

    [Benchmark]
    public int Interface() => A.Value;

    [Benchmark]
    public int Class() => B.Value;
}
