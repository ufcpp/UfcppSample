using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace NBitInt;

public readonly struct Int<BaseInt, Bits> : IBinaryInteger<Int<BaseInt, Bits>>
    where BaseInt : IBinaryInteger<BaseInt>
    where Bits : IConstant<int>
{
    private static BaseInt Modulo => BaseInt.One << Bits.Value;
    private static BaseInt Mask => Modulo - BaseInt.One;

    public BaseInt Value { get; }
    public Int(BaseInt value) => Value = value & Mask;

    private static Int<BaseInt, Bits> Checked(BaseInt value)
    {
        if (value >= Modulo) throw new OverflowException();
        return new(value);
    }

    public static int Radix => BaseInt.Radix;

    public static Int<BaseInt, Bits> Zero => new(BaseInt.Zero);
    public static Int<BaseInt, Bits> One => new(BaseInt.One);

    public static Int<BaseInt, Bits> AdditiveIdentity => new(BaseInt.AdditiveIdentity);
    public static Int<BaseInt, Bits> MultiplicativeIdentity => new(BaseInt.MultiplicativeIdentity);

    public static Int<BaseInt, Bits> Abs(Int<BaseInt, Bits> value)
        => new(BaseInt.Abs(value.Value));
    public static Int<BaseInt, Bits> Clamp(Int<BaseInt, Bits> value, Int<BaseInt, Bits> min, Int<BaseInt, Bits> max)
        => new(BaseInt.Clamp(value.Value, min.Value, max.Value));

    public static (Int<BaseInt, Bits> Quotient, Int<BaseInt, Bits> Remainder) DivRem(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right)
    {
        var (q, r) = BaseInt.DivRem(left.Value, right.Value);
        return (new(q), new(r));
    }

    public static bool IsCanonical(Int<BaseInt, Bits> value) => BaseInt.IsCanonical(value.Value);
    public static bool IsComplexNumber(Int<BaseInt, Bits> value) => BaseInt.IsComplexNumber(value.Value);
    public static bool IsEvenInteger(Int<BaseInt, Bits> value) => BaseInt.IsEvenInteger(value.Value);
    public static bool IsFinite(Int<BaseInt, Bits> value) => BaseInt.IsFinite(value.Value);
    public static bool IsImaginaryNumber(Int<BaseInt, Bits> value) => BaseInt.IsImaginaryNumber(value.Value);
    public static bool IsInfinity(Int<BaseInt, Bits> value) => BaseInt.IsInfinity(value.Value);
    public static bool IsInteger(Int<BaseInt, Bits> value) => BaseInt.IsInteger(value.Value);
    public static bool IsNaN(Int<BaseInt, Bits> value) => BaseInt.IsNaN(value.Value);
    public static bool IsNegative(Int<BaseInt, Bits> value) => BaseInt.IsNegative(value.Value);
    public static bool IsNegativeInfinity(Int<BaseInt, Bits> value) => BaseInt.IsNegativeInfinity(value.Value);
    public static bool IsNormal(Int<BaseInt, Bits> value) => BaseInt.IsNormal(value.Value);
    public static bool IsOddInteger(Int<BaseInt, Bits> value) => BaseInt.IsOddInteger(value.Value);
    public static bool IsPositive(Int<BaseInt, Bits> value) => BaseInt.IsPositive(value.Value);
    public static bool IsPositiveInfinity(Int<BaseInt, Bits> value) => BaseInt.IsPositiveInfinity(value.Value);
    public static bool IsPow2(Int<BaseInt, Bits> value) => BaseInt.IsPow2(value.Value);
    public static bool IsRealNumber(Int<BaseInt, Bits> value) => BaseInt.IsRealNumber(value.Value);
    public static bool IsSubnormal(Int<BaseInt, Bits> value) => BaseInt.IsSubnormal(value.Value);
    public static bool IsZero(Int<BaseInt, Bits> value) => BaseInt.IsZero(value.Value);

    public static Int<BaseInt, Bits> Log2(Int<BaseInt, Bits> value) => new(BaseInt.Log2(value.Value));
    public static Int<BaseInt, Bits> Max(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.Max(x.Value, y.Value));
    public static Int<BaseInt, Bits> MaxMagnitude(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MaxMagnitude(x.Value, y.Value));
    public static Int<BaseInt, Bits> MaxMagnitudeNumber(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MaxMagnitudeNumber(x.Value, y.Value));
    public static Int<BaseInt, Bits> MaxNumber(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MaxNumber(x.Value, y.Value));
    public static Int<BaseInt, Bits> Min(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.Min(x.Value, y.Value));
    public static Int<BaseInt, Bits> MinMagnitude(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MinMagnitude(x.Value, y.Value));
    public static Int<BaseInt, Bits> MinMagnitudeNumber(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MinMagnitudeNumber(x.Value, y.Value));
    public static Int<BaseInt, Bits> MinNumber(Int<BaseInt, Bits> x, Int<BaseInt, Bits> y) => new(BaseInt.MinNumber(x.Value, y.Value));

    static Int<BaseInt, Bits> INumber<Int<BaseInt, Bits>>.CopySign(Int<BaseInt, Bits> value, Int<BaseInt, Bits> sign) => throw new NotImplementedException();
    static int INumber<Int<BaseInt, Bits>>.Sign(Int<BaseInt, Bits> value) => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertFromChecked<TOther>(TOther value, out Int<BaseInt, Bits> result) => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertFromSaturating<TOther>(TOther value, out Int<BaseInt, Bits> result) => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertFromTruncating<TOther>(TOther value, out Int<BaseInt, Bits> result) => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertToChecked<TOther>(Int<BaseInt, Bits> value, [NotNullWhen(true)] out TOther? result) where TOther : default => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertToSaturating<TOther>(Int<BaseInt, Bits> value, [NotNullWhen(true)] out TOther? result) where TOther : default => throw new NotImplementedException();
    static bool INumberBase<Int<BaseInt, Bits>>.TryConvertToTruncating<TOther>(Int<BaseInt, Bits> value, [NotNullWhen(true)] out TOther? result) where TOther : default => throw new NotImplementedException();
    static Int<BaseInt, Bits> IBinaryInteger<Int<BaseInt, Bits>>.LeadingZeroCount(Int<BaseInt, Bits> value) => throw new NotImplementedException();
    static Int<BaseInt, Bits> IBinaryInteger<Int<BaseInt, Bits>>.TrailingZeroCount(Int<BaseInt, Bits> value) => throw new NotImplementedException();
    static Int<BaseInt, Bits> IBinaryInteger<Int<BaseInt, Bits>>.PopCount(Int<BaseInt, Bits> value) => throw new NotImplementedException();
    static Int<BaseInt, Bits> IBinaryInteger<Int<BaseInt, Bits>>.RotateLeft(Int<BaseInt, Bits> value, int rotateAmount) => throw new NotImplementedException();
    static Int<BaseInt, Bits> IBinaryInteger<Int<BaseInt, Bits>>.RotateRight(Int<BaseInt, Bits> value, int rotateAmount) => throw new NotImplementedException();
    int IBinaryInteger<Int<BaseInt, Bits>>.GetByteCount() => throw new NotImplementedException();
    int IBinaryInteger<Int<BaseInt, Bits>>.GetShortestBitLength() => throw new NotImplementedException();

    public static Int<BaseInt, Bits> operator +(Int<BaseInt, Bits> value) => new(+value.Value);
    public static Int<BaseInt, Bits> operator +(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value + right.Value);
    public static Int<BaseInt, Bits> operator checked +(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => Checked(left.Value + right.Value);
    public static Int<BaseInt, Bits> operator -(Int<BaseInt, Bits> value) => new(-value.Value);
    public static Int<BaseInt, Bits> operator checked -(Int<BaseInt, Bits> value) => Checked(-value.Value);
    public static Int<BaseInt, Bits> operator -(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value - right.Value);
    public static Int<BaseInt, Bits> operator checked -(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => Checked(left.Value - right.Value);

    public static Int<BaseInt, Bits> operator ++(Int<BaseInt, Bits> value) => new(value.Value + BaseInt.One);
    public static Int<BaseInt, Bits> operator checked ++(Int<BaseInt, Bits> value) => Checked(value.Value + BaseInt.One);
    public static Int<BaseInt, Bits> operator --(Int<BaseInt, Bits> value) => new(value.Value - BaseInt.One);
    public static Int<BaseInt, Bits> operator checked --(Int<BaseInt, Bits> value) => Checked(value.Value - BaseInt.One);

    public static Int<BaseInt, Bits> operator *(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value * right.Value);
    public static Int<BaseInt, Bits> operator checked *(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => Checked(left.Value * right.Value);
    public static Int<BaseInt, Bits> operator /(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value / right.Value);
    public static Int<BaseInt, Bits> operator checked /(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => Checked(left.Value / right.Value);
    public static Int<BaseInt, Bits> operator %(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value % right.Value);

    public static Int<BaseInt, Bits> operator ~(Int<BaseInt, Bits> value) => new(~value.Value);
    public static Int<BaseInt, Bits> operator &(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value & right.Value);
    public static Int<BaseInt, Bits> operator |(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value | right.Value);
    public static Int<BaseInt, Bits> operator ^(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => new(left.Value ^ right.Value);

    public static Int<BaseInt, Bits> operator <<(Int<BaseInt, Bits> value, int shiftAmount) => new(value.Value << shiftAmount);
    public static Int<BaseInt, Bits> operator >>(Int<BaseInt, Bits> value, int shiftAmount) => new(value.Value >> shiftAmount);
    public static Int<BaseInt, Bits> operator >>>(Int<BaseInt, Bits> value, int shiftAmount) => new(value.Value >>> shiftAmount);

    public static bool operator ==(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value == right.Value;
    public static bool operator !=(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value != right.Value;

    public static bool operator <(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value < right.Value;
    public static bool operator >(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value > right.Value;
    public static bool operator <=(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value <= right.Value;
    public static bool operator >=(Int<BaseInt, Bits> left, Int<BaseInt, Bits> right) => left.Value >= right.Value;

    public int CompareTo(object? obj) => obj is Int<BaseInt, Bits> other ? CompareTo(other) : -1;
    public int CompareTo(Int<BaseInt, Bits> other) => Value.CompareTo(other.Value);
    public bool Equals(Int<BaseInt, Bits> other) => Value == other.Value;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Int<BaseInt, Bits> other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => Value.TryFormat(destination, out charsWritten, format, provider);

    public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
        => Value.TryWriteBigEndian(destination, out bytesWritten);
    public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
        => Value.TryWriteLittleEndian(destination, out bytesWritten);

    public static Int<BaseInt, Bits> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        => new(BaseInt.Parse(s, style, provider));
    public static Int<BaseInt, Bits> Parse(string s, NumberStyles style, IFormatProvider? provider)
        => new(BaseInt.Parse(s, style, provider));
    public static Int<BaseInt, Bits> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => new(BaseInt.Parse(s, provider));
    public static Int<BaseInt, Bits> Parse(string s, IFormatProvider? provider)
        => new(BaseInt.Parse(s, provider));

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Int<BaseInt, Bits> result)
    {
        if (BaseInt.TryParse(s, style, provider, out var v))
        {
            result = new(v);
            return true;
        }
        else
        {
            result = Zero;
            return false;
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Int<BaseInt, Bits> result)
    {
        if (BaseInt.TryParse(s, style, provider, out var v))
        {
            result = new(v);
            return true;
        }
        else
        {
            result = Zero;
            return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Int<BaseInt, Bits> result)
    {
        if (BaseInt.TryParse(s, provider, out var v))
        {
            result = new(v);
            return true;
        }
        else
        {
            result = Zero;
            return false;
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Int<BaseInt, Bits> result)
    {
        if (BaseInt.TryParse(s, provider, out var v))
        {
            result = new(v);
            return true;
        }
        else
        {
            result = Zero;
            return false;
        }
    }
}
