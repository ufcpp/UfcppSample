using Algebra;
using System.Numerics;

var r1 = new Rational<int>(-35, 10);
var r2 = new Rational<int>(7, -2);

Console.WriteLine($"{r1} == {r2} {r1 == r2}");
Console.WriteLine($"{r1 + r2}, {r1 - r2}, {r1 * r2}, {r1 / r2}");
