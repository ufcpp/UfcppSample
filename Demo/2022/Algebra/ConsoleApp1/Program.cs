using Algebra;

var r = new Random();
var (a, b) = (r.Next(1, 1000), r.Next(1, 1000));
//var (a, b) = (10, 13);
//var (a, b) = (5 * 7 * 13, 2 * 3 * 5 * 7);

Console.WriteLine(EuclideanAlgorithm.Gcd(a, b));

var (g, x, y) = EuclideanAlgorithm.Egcd(a, b);

Console.WriteLine(g);
Console.WriteLine($"{a}・{x} + {b}・{y} = {a * x + b * y}");
