using System.Numerics;

namespace Algebra.Constants;

public struct RationalConstant<TBase, N> : IConstant<Rational<TBase>>
    where TBase : IAdditiveIdentity<TBase, TBase>,
    IMultiplicativeIdentity<TBase, TBase>,
    IAdditionOperators<TBase, TBase, TBase>,
    ISubtractionOperators<TBase, TBase, TBase>,
    IMultiplyOperators<TBase, TBase, TBase>,
    IDivisionOperators<TBase, TBase, TBase>,
    IUnaryNegationOperators<TBase, TBase>,
    IComparisonOperators<TBase, TBase>
    where N : IConstant<TBase>
{
    public static Rational<TBase> Value { get; } = new(N.Value);
}
