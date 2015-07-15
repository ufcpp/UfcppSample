public struct SampleStruct : InterfaceA, InterfaceB
{
    public int A { get; }
    public int B { get; }

    public SampleStruct(int a, int b) { A = a; B = b; }

    public static SampleStruct operator-(SampleStruct x)
        => new SampleStruct(-x.A, -x.B);
}

public interface InterfaceA { int A { get; } }
public interface InterfaceB { int B { get; } }
