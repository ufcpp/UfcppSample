namespace Algebra.Constants;

public interface IConstant<T>
{
    static abstract T Value { get; }
}

public struct Constant<T, TBase, N> : IConstant<T>
    where T : IImplicitConversion<T, TBase>
    where N : IConstant<TBase>
{
    public static T Value { get; } = N.Value;
}
