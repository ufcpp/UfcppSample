namespace Algebra.Constants;

public interface IConstant<T>
{
    static abstract T Value { get; }
}
