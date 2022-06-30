using Algebra;
using System.Numerics;

using Q2 = Algebra.QuadraticField<
    Algebra.Rational<short>,
    Algebra.Constants.RationalConstants<short>._2>;

using C = Algebra.QuadraticField<
    Algebra.Rational<short>,
    Algebra.Constants.RationalConstants<short>.M1>;

using static Algebra.RationalQuadraticField<long, Algebra.Constants.M1>;
using Algebra.Constants;
{
    var r1 = new Rational<BigInteger>(-35, 10);
    var r2 = new Rational<BigInteger>(7, -2);

    Console.WriteLine($"{r1} == {r2} {r1 == r2}");
    Console.WriteLine($"{r1 + r2}, {r1 - r2}, {r1 * r2}, {r1 / r2}");

    Console.WriteLine(r1 / 7);
    Console.WriteLine(-r1 * 2 / 3);
}
{
    var r1 = new Rational<int>(-35, 10);
    var r2 = new Rational<int>(7, -2);

    Console.WriteLine($"{r1} == {r2} {r1 == r2}");
    Console.WriteLine($"{r1 + r2}, {r1 - r2}, {r1 * r2}, {r1 / r2}");

    Console.WriteLine(r1 / 7);
    Console.WriteLine(-r1 * 2 / 3);
}
{
    var x = new Q2(1);
    var y = new Q2(0, -1);

    Console.WriteLine(new Q2(0));
    Console.WriteLine(new Q2(1));
    Console.WriteLine(new Q2(0, 1));
    Console.WriteLine(x);
    Console.WriteLine(y);
    var z = x + y;
    Console.WriteLine(z);
    Console.WriteLine(z * z);
    Console.WriteLine(z / new Q2(3, 5));
}
Console.WriteLine("-----");
{
    var x = new C(1);
    var y = new C(0, -1);

    Console.WriteLine(new C(0));
    Console.WriteLine(new C(1));
    Console.WriteLine(new C(0, 1));
    Console.WriteLine(x);
    Console.WriteLine(y);
    var z = x + y;
    Console.WriteLine(z);
    Console.WriteLine(z * z);
    Console.WriteLine(z / new C(3, 5));
}

{
    var x = 3 + I * 2;
    var y = -4 + I * 3;

    Console.WriteLine(x);
    Console.WriteLine(y);
    Console.WriteLine(x + y);
    Console.WriteLine(x - y);
    Console.WriteLine(x * y);
    Console.WriteLine(x / y);
}

{
    var q = 1.AsQ();

    var q1 = 1.AsQ(new _5());
    //var q2 = q1.AsQ(new _7());
}

static class Ex
{
    public static Rational<TBase> AsQ<TBase>(this TBase x)
        where TBase :
        IAdditiveIdentity<TBase, TBase>,
        IMultiplicativeIdentity<TBase, TBase>,
        IAdditionOperators<TBase, TBase, TBase>,
        ISubtractionOperators<TBase, TBase, TBase>,
        IMultiplyOperators<TBase, TBase, TBase>,
        IDivisionOperators<TBase, TBase, TBase>,
        IUnaryNegationOperators<TBase, TBase>,
        IComparisonOperators<TBase, TBase>
        => new(x);

    public static QuadraticField<Rational<TBase>, RationalConstant<TBase, IntegerConstant<TBase, N>>> AsQ<TBase, N>(this TBase x, N _)
        where TBase : INumber<TBase>
        where N : IConstant<int>
        => new(new(x));
}

public static class ConstantTypeBuilder
{
    public static Constant<Rational<TBase>, TBase, N> Rational<TBase, N>()
        where TBase :
        IAdditiveIdentity<TBase, TBase>,
        IMultiplicativeIdentity<TBase, TBase>,
        IAdditionOperators<TBase, TBase, TBase>,
        ISubtractionOperators<TBase, TBase, TBase>,
        IMultiplyOperators<TBase, TBase, TBase>,
        IDivisionOperators<TBase, TBase, TBase>,
        IUnaryNegationOperators<TBase, TBase>,
        IComparisonOperators<TBase, TBase>
        where N : IConstant<TBase>
        => default;
}
