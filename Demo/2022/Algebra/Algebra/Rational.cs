using System.Numerics;

namespace Algebra;

public readonly struct Rational<TBase>
    : IAdditiveIdentity<Rational<TBase>, Rational<TBase>>,
    IMultiplicativeIdentity<Rational<TBase>, Rational<TBase>>,
    IAdditionOperators<Rational<TBase>, Rational<TBase>, Rational<TBase>>,
    IAdditionOperators<Rational<TBase>, TBase, Rational<TBase>>,
    ISubtractionOperators<Rational<TBase>, Rational<TBase>, Rational<TBase>>,
    ISubtractionOperators<Rational<TBase>, TBase, Rational<TBase>>,
    IMultiplyOperators<Rational<TBase>, Rational<TBase>, Rational<TBase>>,
    IMultiplyOperators<Rational<TBase>, TBase, Rational<TBase>>,
    IDivisionOperators<Rational<TBase>, Rational<TBase>, Rational<TBase>>,
    IDivisionOperators<Rational<TBase>, TBase, Rational<TBase>>,
    IComparisonOperators<Rational<TBase>, Rational<TBase>>,
    IComparisonOperators<Rational<TBase>, TBase>,
    IUnaryPlusOperators<Rational<TBase>, Rational<TBase>>,
    IUnaryNegationOperators<Rational<TBase>, Rational<TBase>>,
    IImplicitConversion<Rational<TBase>, TBase>

    where TBase :
    IAdditiveIdentity<TBase, TBase>,
    IMultiplicativeIdentity<TBase, TBase>,
    IAdditionOperators<TBase, TBase, TBase>,
    ISubtractionOperators<TBase, TBase, TBase>,
    IMultiplyOperators<TBase, TBase, TBase>,
    IDivisionOperators<TBase, TBase, TBase>,
    IUnaryNegationOperators<TBase, TBase>,
    IComparisonOperators<TBase, TBase>
{
    public TBase Numerator { get; }
    public TBase Denominator { get; }

    public Rational(TBase value)
    {
        (Numerator, Denominator) = (value, TBase.MultiplicativeIdentity);
    }

    public Rational(TBase numerator, TBase denominator)
    {
        (Numerator, Denominator) = Canonicalize(numerator, denominator);
    }

    public static (TBase x, TBase y) Canonicalize(TBase a, TBase b)
    {
        var (r, r0) = (b, a);
        var (s, s0) = (TBase.AdditiveIdentity, TBase.MultiplicativeIdentity);
        var (t, t0) = (TBase.MultiplicativeIdentity, TBase.AdditiveIdentity);

        while (r != TBase.AdditiveIdentity)
        {
            var q = r0 / r;
            (r0, r) = (r, r0 - q * r);
            (s0, s) = (s, s0 - q * s);
            (t0, t) = (t, t0 - q * t);
        }

        if (s == TBase.AdditiveIdentity) return (TBase.MultiplicativeIdentity, TBase.AdditiveIdentity);

        if (s < TBase.AdditiveIdentity)
        {
            s = -s;
            t = -t;
        }

        if (s == TBase.MultiplicativeIdentity) return (-t, TBase.MultiplicativeIdentity);

        return (-t, s);
    }

    public static implicit operator Rational<TBase>(TBase value) => new(value);

    public static Rational<TBase> One { get; } = new(TBase.MultiplicativeIdentity, TBase.MultiplicativeIdentity);
    public static Rational<TBase> Zero { get; } = new(TBase.AdditiveIdentity, TBase.MultiplicativeIdentity);

    static Rational<TBase> IAdditiveIdentity<Rational<TBase>, Rational<TBase>>.AdditiveIdentity => Zero;
    static Rational<TBase> IMultiplicativeIdentity<Rational<TBase>, Rational<TBase>>.MultiplicativeIdentity => One;

    public static Rational<TBase> operator +(Rational<TBase> left, Rational<TBase> right)
        => new(left.Numerator * right.Denominator + left.Denominator * right.Numerator, left.Denominator * right.Denominator);

    public static Rational<TBase> operator checked +(Rational<TBase> left, Rational<TBase> right)
        => checked(new(left.Numerator * right.Denominator + left.Denominator * right.Numerator, left.Denominator * right.Denominator));

    public static Rational<TBase> operator +(Rational<TBase> left, TBase right)
        => new(left.Numerator + left.Denominator * right, left.Denominator);

    public static Rational<TBase> operator checked +(Rational<TBase> left, TBase right)
        => checked(new(left.Numerator + left.Denominator * right, left.Denominator));

    public static Rational<TBase> operator +(TBase left, Rational<TBase> right)
        => new(left * right.Denominator + right.Numerator, right.Denominator);

    public static Rational<TBase> operator checked +(TBase left, Rational<TBase> right)
        => checked(new(left * right.Denominator + right.Numerator, right.Denominator));

    public static Rational<TBase> operator -(Rational<TBase> left, Rational<TBase> right)
        => new(left.Numerator * right.Denominator - left.Denominator * right.Numerator, left.Denominator * right.Denominator);

    public static Rational<TBase> operator checked -(Rational<TBase> left, Rational<TBase> right)
        => checked(new(left.Numerator * right.Denominator - left.Denominator * right.Numerator, left.Denominator * right.Denominator));

    public static Rational<TBase> operator -(Rational<TBase> left, TBase right)
        => new(left.Numerator - left.Denominator * right, left.Denominator);

    public static Rational<TBase> operator checked -(Rational<TBase> left, TBase right)
        => checked(new(left.Numerator - left.Denominator * right, left.Denominator));

    public static Rational<TBase> operator -(TBase left, Rational<TBase> right)
        => new(left * right.Denominator - right.Numerator, right.Denominator);

    public static Rational<TBase> operator checked -(TBase left, Rational<TBase> right)
        => checked(new(left * right.Denominator - right.Numerator, right.Denominator));

    public static Rational<TBase> operator *(Rational<TBase> left, Rational<TBase> right)
        => new(left.Numerator * right.Numerator, left.Denominator * right.Denominator);

    public static Rational<TBase> operator checked *(Rational<TBase> left, Rational<TBase> right)
        => checked(new(left.Numerator * right.Numerator, left.Denominator * right.Denominator));

    public static Rational<TBase> operator *(Rational<TBase> left, TBase right)
        => new(left.Numerator * right, left.Denominator);

    public static Rational<TBase> operator checked *(Rational<TBase> left, TBase right)
        => checked(new(left.Numerator * right, left.Denominator));

    public static Rational<TBase> operator *(TBase left, Rational<TBase> right)
        => new(left * right.Numerator, right.Denominator);

    public static Rational<TBase> operator checked *(TBase left, Rational<TBase> right)
        => checked(new(left * right.Numerator, right.Denominator));

    public static Rational<TBase> operator /(Rational<TBase> left, Rational<TBase> right)
        => checked(new(left.Numerator * right.Denominator, left.Denominator * right.Numerator));

    public static Rational<TBase> operator checked /(Rational<TBase> left, Rational<TBase> right)
        => checked(new(left.Numerator * right.Denominator, left.Denominator * right.Numerator));

    public static Rational<TBase> operator /(Rational<TBase> left, TBase right)
        => new(left.Numerator, left.Denominator * right);

    public static Rational<TBase> operator checked /(Rational<TBase> left, TBase right)
        => checked(new(left.Numerator, left.Denominator * right));

    public static Rational<TBase> operator /(TBase left, Rational<TBase> right)
        => checked(new(left * right.Denominator, right.Numerator));

    public static Rational<TBase> operator checked /(TBase left, Rational<TBase> right)
        => checked(new(left * right.Denominator, right.Numerator));

    public static Rational<TBase> operator +(Rational<TBase> value) => value;

    public static Rational<TBase> operator checked -(Rational<TBase> value) => checked(new(-value.Numerator, value.Denominator));

    public static Rational<TBase> operator -(Rational<TBase> value) => new(-value.Numerator, value.Denominator);

    public static bool operator ==(Rational<TBase> left, Rational<TBase> right) => left.Equals(right);
    public static bool operator !=(Rational<TBase> left, Rational<TBase> right) => !left.Equals(right);
    public static bool operator >(Rational<TBase> left, Rational<TBase> right) => left.Numerator * right.Denominator > left.Denominator * right.Numerator;
    public static bool operator >=(Rational<TBase> left, Rational<TBase> right) => left.Numerator * right.Denominator >= left.Denominator * right.Numerator;
    public static bool operator <(Rational<TBase> left, Rational<TBase> right) => left.Numerator * right.Denominator < left.Denominator * right.Numerator;
    public static bool operator <=(Rational<TBase> left, Rational<TBase> right) => left.Numerator * right.Denominator <= left.Denominator * right.Numerator;
    public bool Equals(Rational<TBase> other) => Numerator * other.Denominator == Denominator * other.Numerator;
    public int CompareTo(Rational<TBase> other) => (Numerator * other.Denominator).CompareTo(Denominator * other.Denominator);

    public static bool operator ==(Rational<TBase> left, TBase right) => left.Equals(right);
    public static bool operator !=(Rational<TBase> left, TBase right) => !left.Equals(right);
    public static bool operator >(Rational<TBase> left, TBase right) => left.Numerator > left.Denominator * right;
    public static bool operator >=(Rational<TBase> left, TBase right) => left.Numerator >= left.Denominator * right;
    public static bool operator <(Rational<TBase> left, TBase right) => left.Numerator < left.Denominator * right;
    public static bool operator <=(Rational<TBase> left, TBase right) => left.Numerator <= left.Denominator * right;
    public bool Equals(TBase? other) => Numerator == Denominator * other!;
    public int CompareTo(TBase? other) => Numerator.CompareTo(Denominator * other!);

    public static bool operator ==(TBase left, Rational<TBase> right) => left.Equals(right);
    public static bool operator !=(TBase left, Rational<TBase> right) => !left.Equals(right);
    public static bool operator >(TBase left, Rational<TBase> right) => left * right.Denominator > right.Numerator;
    public static bool operator >=(TBase left, Rational<TBase> right) => left * right.Denominator >= right.Numerator;
    public static bool operator <(TBase left, Rational<TBase> right) => left * right.Denominator < right.Numerator;
    public static bool operator <=(TBase left, Rational<TBase> right) => left * right.Denominator <= right.Numerator;

    public override bool Equals(object? obj) => obj is Rational<TBase> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
    public int CompareTo(object? obj) => obj is Rational<TBase> other ? CompareTo(other) : -1;
    public override string ToString() => Denominator == TBase.MultiplicativeIdentity
        ? Numerator.ToString()!
        : $"{Numerator}/{Denominator}";
}
