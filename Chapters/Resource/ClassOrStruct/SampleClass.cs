public sealed class SampleClass : BaseClass, InterfaceA, InterfaceB
{
    public int A { get; set; }
    public int B { get; set; }

    public SampleClass() { }
    public SampleClass(int a, int b) { A = a; B = b; }

    public static SampleClass operator -(SampleClass x)
        => new SampleClass(-x.A, -x.B);

    public override void X() { }

    ~SampleClass() { }
}

public class BaseClass
{
    public virtual void X() { }
}

public static class StaticClass
{
    public static string Hex(int x) => x.ToString("X");
}
