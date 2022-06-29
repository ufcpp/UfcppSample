using System.Numerics;
using Algebra.Constants;

namespace Algebra;

public static class RationalQuadraticField<TBase, N>
    where TBase : INumber<TBase>
    where N : IConstant<int>
{
    public static QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>> Zero { get; } = QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>>.Zero;
    public static QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>> One { get; } = QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>>.One;
    public static QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>> I { get; } = QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>>.I;
}
