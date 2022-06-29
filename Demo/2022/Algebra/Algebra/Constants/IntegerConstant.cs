using System.Numerics;

namespace Algebra.Constants;

public struct IntegerConstant<T, N> : IConstant<T>
    where T : INumberBase<T>
    where N : IConstant<int>
{
    public static T Value { get; } = T.CreateChecked(N.Value);
}
