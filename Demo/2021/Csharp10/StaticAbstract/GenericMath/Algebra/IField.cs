namespace GenericMath.Algebra;

public interface IField<TSelf>
    : IAdditionOperators<TSelf, TSelf, TSelf>,
    IAdditiveIdentity<TSelf, TSelf>,
    IEqualityOperators<TSelf, TSelf>,
    IEquatable<TSelf>,
    IDecrementOperators<TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IIncrementOperators<TSelf>,
    IMultiplicativeIdentity<TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>,
    //IFormattable,
    //ISpanFormattable,
    //IParseable<TSelf>,
    //ISpanParseable<TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IUnaryNegationOperators<TSelf, TSelf>,
    IUnaryPlusOperators<TSelf, TSelf>
    where TSelf : IField<TSelf>
{
}
