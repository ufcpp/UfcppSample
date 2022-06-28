using Algebra;
using System.Numerics;

namespace TestAlgebra;

public class EuclideanAlgorithmTest
{
    public static IEnumerable<object[]> GetRandomData()
    {
        const int min = 1;
        const int max = 500;
        var r = new Random();

        return Enumerable.Range(0, 100)
            .Select(_ => new object[]
            {
                r.Next(min, max),
                r.Next(min, max),
            })
            .ToArray();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 6)]
    [InlineData(6, 3)]
    [InlineData(10, 13)]
    [InlineData(5 * 7 * 13, 2 * 3 * 5 * 7)]
    [MemberData(nameof(GetRandomData))]
    public void Gcd(int a, int b)
    {
        GcdT<byte>(a, b);
        GcdT<sbyte>(a, b);
        GcdT<short>(a, b);
        GcdT<ushort>(a, b);
        GcdT<int>(a, b);
        GcdT<uint>(a, b);
        GcdT<long>(a, b);
        GcdT<ulong>(a, b);
        GcdT<Int128>(a, b);
        GcdT<UInt128>(a, b);
    }

    private static void GcdT<T>(int a0, int b0)
        where T : INumber<T>
    {
        var a = T.CreateTruncating(a0);
        var b = T.CreateTruncating(b0);

        var g1 = EuclideanAlgorithm.Gcd(a, b);
        var g2 = EuclideanAlgorithm.GcdModulus(a, b);
        var (g3, x, y) = EuclideanAlgorithm.Egcd(a, b);

        Assert.Equal(g1, g2);
        Assert.Equal(g1, g3);
        Assert.Equal(g1, a * x + b * y);
    }
}