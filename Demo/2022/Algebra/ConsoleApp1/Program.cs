using Algebra;
using System.Numerics;

var r = new Random();

writeRandom<byte>(1, 500);
writeRandom<sbyte>(1, 500);
writeRandom<short>(1, 10000);
writeRandom<ushort>(1, 10000);
writeRandom<int>(1, 10000);
writeRandom<uint>(1, 10000);

write(10, 13);
write(5 * 7 * 13, 2 * 3 * 5 * 7);

void writeRandom<T>(int min, int max)
    where T : INumber<T>
{
    var a = r.Next(min, max);
    var b = r.Next(min, max);
    write(T.CreateTruncating(a), T.CreateTruncating(b));
}

static void write<T>(T a, T b)
    where T : INumber<T>
{
    var g1 = EuclideanAlgorithm.Gcd(a, b);
    var g2 = EuclideanAlgorithm.GcdModulus(a, b);

    var (g3, x, y) = EuclideanAlgorithm.Egcd(a, b);

    Console.WriteLine($"{a}・{x} + {b}・{y} = {a * x + b * y}, {g1}, {g2}, {g3}");
}
