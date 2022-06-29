using System.Numerics;
using Algebra.Constants;

namespace Algebra;

public readonly struct QuadraticField<TBase, D>
    : IAdditiveIdentity<QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IMultiplicativeIdentity<QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IAdditionOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IAdditionOperators<QuadraticField<TBase, D>, TBase, QuadraticField<TBase, D>>,
    ISubtractionOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    ISubtractionOperators<QuadraticField<TBase, D>, TBase, QuadraticField<TBase, D>>,
    IMultiplyOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IMultiplyOperators<QuadraticField<TBase, D>, TBase, QuadraticField<TBase, D>>,
    IDivisionOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IDivisionOperators<QuadraticField<TBase, D>, TBase, QuadraticField<TBase, D>>,
    IEqualityOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IEqualityOperators<QuadraticField<TBase, D>, TBase>,
    IUnaryPlusOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>>,
    IUnaryNegationOperators<QuadraticField<TBase, D>, QuadraticField<TBase, D>>

    where TBase : IAdditiveIdentity<TBase, TBase>,
    IMultiplicativeIdentity<TBase, TBase>,
    IAdditionOperators<TBase, TBase, TBase>,
    ISubtractionOperators<TBase, TBase, TBase>,
    IMultiplyOperators<TBase, TBase, TBase>,
    IDivisionOperators<TBase, TBase, TBase>,
    IEqualityOperators<TBase, TBase>,
    IUnaryPlusOperators<TBase, TBase>,
    IUnaryNegationOperators<TBase, TBase>,
    IComparisonOperators<TBase, TBase>

    where D : IConstant<TBase>
{
    public TBase X { get; }
    public TBase Y { get; }

    public QuadraticField(TBase x) : this(x, TBase.AdditiveIdentity) { }

    public QuadraticField(TBase x, TBase y) => (X, Y) = (x, y);

    public static implicit operator QuadraticField<TBase, D>(TBase value) => new(value);

    public static QuadraticField<TBase, D> One { get; } = new(TBase.MultiplicativeIdentity, TBase.AdditiveIdentity);
    public static QuadraticField<TBase, D> Zero { get; } = new(TBase.AdditiveIdentity, TBase.AdditiveIdentity);

    static QuadraticField<TBase, D> IAdditiveIdentity<QuadraticField<TBase, D>, QuadraticField<TBase, D>>.AdditiveIdentity => Zero;
    static QuadraticField<TBase, D> IMultiplicativeIdentity<QuadraticField<TBase, D>, QuadraticField<TBase, D>>.MultiplicativeIdentity => One;

    public static QuadraticField<TBase, D> operator +(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => new(left.X + right.X, left.Y + right.Y);

    public static QuadraticField<TBase, D> operator checked +(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => checked(new(left.X + right.X, left.Y + right.Y));

    public static QuadraticField<TBase, D> operator +(QuadraticField<TBase, D> left, TBase right)
        => new(left.X + right, left.Y);

    public static QuadraticField<TBase, D> operator checked +(QuadraticField<TBase, D> left, TBase right)
        => checked(new(left.X + right, left.Y));

    public static QuadraticField<TBase, D> operator +(TBase left, QuadraticField<TBase, D> right)
        => new(left + right.X, right.Y);

    public static QuadraticField<TBase, D> operator checked +(TBase left, QuadraticField<TBase, D> right)
        => checked(new(left + right.X, right.Y));

    public static QuadraticField<TBase, D> operator -(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => new(left.X - right.X, left.Y - right.Y);

    public static QuadraticField<TBase, D> operator checked -(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => checked(new(left.X - right.X, left.Y - right.Y));

    public static QuadraticField<TBase, D> operator -(QuadraticField<TBase, D> left, TBase right)
        => new(left.X - right, left.Y);

    public static QuadraticField<TBase, D> operator checked -(QuadraticField<TBase, D> left, TBase right)
        => checked(new(left.X - right, left.Y));

    public static QuadraticField<TBase, D> operator -(TBase left, QuadraticField<TBase, D> right)
        => new(left - right.X, right.Y);

    public static QuadraticField<TBase, D> operator checked -(TBase left, QuadraticField<TBase, D> right)
        => checked(new(left - right.X, right.Y));

    public static QuadraticField<TBase, D> operator *(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => new(left.X * right.X + D.Value * left.Y * right.Y, left.X * right.Y + left.Y * right.X);

    public static QuadraticField<TBase, D> operator checked *(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
        => checked(new(left.X * right.X + D.Value * left.Y * right.Y, left.X * right.Y + left.Y * right.X));

    public static QuadraticField<TBase, D> operator *(QuadraticField<TBase, D> left, TBase right)
        => new(left.X * right, left.Y * right);

    public static QuadraticField<TBase, D> operator checked *(QuadraticField<TBase, D> left, TBase right)
        => checked(new(left.X * right, left.Y * right));

    public static QuadraticField<TBase, D> operator *(TBase left, QuadraticField<TBase, D> right)
        => new(left * right.X, left * right.X);

    public static QuadraticField<TBase, D> operator checked *(TBase left, QuadraticField<TBase, D> right)
        => checked(new(left * right.X, left * right.X));

    public static QuadraticField<TBase, D> operator /(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
    {
        var n = right.Norm();
        var t = left * right.Conjugate();
        return new(t.X / n, t.Y / n);
    }

    public static QuadraticField<TBase, D> operator checked /(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right)
    {
        checked
        {
            var n = right.Norm();
            var t = left * right.Conjugate();
            return new(t.X / n, t.Y / n);
        }
    }

    public static QuadraticField<TBase, D> operator /(QuadraticField<TBase, D> left, TBase right)
        => new(left.X / right, left.Y / right);

    public static QuadraticField<TBase, D> operator checked /(QuadraticField<TBase, D> left, TBase right)
        => checked(new(left.X / right, left.Y / right));

    public static QuadraticField<TBase, D> operator /(TBase left, QuadraticField<TBase, D> right)
    {
        var n = right.Norm();
        var t = left * right.Conjugate();
        return new(t.X / n, t.Y / n);
    }

    public static QuadraticField<TBase, D> operator checked /(TBase left, QuadraticField<TBase, D> right)
    {
        checked
        {
            var n = right.Norm();
            var t = left * right.Conjugate();
            return new(t.X / n, t.Y / n);
        }
    }

    public static QuadraticField<TBase, D> operator +(QuadraticField<TBase, D> value) => value;
    public static QuadraticField<TBase, D> operator checked -(QuadraticField<TBase, D> value) => checked(new(-value.X, -value.Y));
    public static QuadraticField<TBase, D> operator -(QuadraticField<TBase, D> value) => new(-value.X, -value.Y);

    public QuadraticField<TBase, D> Conjugate() => new(X, -Y);
    public TBase Norm() => X * X - D.Value * Y * Y;

    public static bool operator ==(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right) => left.Equals(right);
    public static bool operator !=(QuadraticField<TBase, D> left, QuadraticField<TBase, D> right) => !left.Equals(right);
    public bool Equals(QuadraticField<TBase, D> other) => (X, Y) == (other.X, other.Y);

    public static bool operator ==(QuadraticField<TBase, D> left, TBase right) => left.Equals(right);
    public static bool operator !=(QuadraticField<TBase, D> left, TBase right) => !left.Equals(right);
    public bool Equals(TBase? other) => (X, Y) == (other, TBase.AdditiveIdentity);

    public override bool Equals(object? obj) => obj is QuadraticField<TBase, D> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString()
        =>
        Y == TBase.AdditiveIdentity ? $"{X}" :
        X == TBase.AdditiveIdentity ? $"{Y}√{D.Value}" :
        Y < TBase.AdditiveIdentity ? $"{X}{Y}√{D.Value}" :
        $"{X}+{Y}√{D.Value}";
}
