using Algebra;
using System.Numerics;

using Q2 = Algebra.QuadraticField<
    Algebra.Rational<short>,
    Algebra.Constants.RationalConstants<short>._2>;

using C = Algebra.QuadraticField<
    Algebra.Rational<short>,
    Algebra.Constants.RationalConstants<short>.M1>;

using static Algebra.RationalQuadraticField<long, Algebra.Constants.M1>;

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
