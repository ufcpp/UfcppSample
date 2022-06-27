using System.Numerics;

namespace Algebra;

public class EuclideanAlgorithm
{
    public static T Gcd<T>(T a, T b)
        where T : IAdditiveIdentity<T, T>,
        IModulusOperators<T, T, T>,
        IEqualityOperators<T, T>
    {
        if (b == T.AdditiveIdentity) return a;
        return Gcd(b, a % b);
    }

    public static (T gcd, T x, T y) Egcd<T>(T a, T b)
        where T : IAdditiveIdentity<T, T>,
        IMultiplicativeIdentity<T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplyOperators<T, T, T>,
        IDivisionOperators<T, T, T>,
        IEqualityOperators<T, T>
    {
        var (r, r0) = (b, a);
        var (s, s0) = (T.AdditiveIdentity, T.MultiplicativeIdentity);
        var (t, t0) = (T.MultiplicativeIdentity, T.AdditiveIdentity);

        while (r != T.AdditiveIdentity)
        {
            var q = r0 / r;
            (r0, r) = (r, r0 - q * r);
            (s0, s) = (s, s0 - q * s);
            (t0, t) = (t, t0 - q * t);
        }

        return (r0, s0, t0);
    }
}
