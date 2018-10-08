interface I<T>
{
    T[]? X { get; }
}

abstract class Base<T> : I<T>
{
    public virtual T[]? X => null;

    class Inner : Base<T>
    {
        public override T[]? X => null; // CS8608
    }
}

class Outer : Base<string>
{
    public override string[]? X => null; // No warning
}
