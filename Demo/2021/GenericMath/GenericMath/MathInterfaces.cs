// https://github.com/tannergooding/runtime/blob/6778b79a819aef37fb886ec8bc2e74e65ff95a25/src/libraries/System.Private.CoreLib/src/System/IAdditionOperators.cs
// とかからのコピー品。
// .NET 6 Preview 7 とか RC とかに入るはずだけど、 Preview 6 にはマージされてなさそう。
// いったん手作業コピー。

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;

namespace System
{
    [RequiresPreviewFeatures]
    public interface IAdditionOperators<TSelf, TOther, TResult>
        where TSelf : IAdditionOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator +(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IAdditiveIdentity<TSelf, TResult>
        where TSelf : IAdditiveIdentity<TSelf, TResult>
    {
        static abstract TResult AdditiveIdentity { get; }
    }

    [RequiresPreviewFeatures]
    public interface IBitwiseOperators<TSelf, TOther, TResult>
        where TSelf : IBitwiseOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator &(TSelf left, TOther right);
        static abstract TResult operator |(TSelf left, TOther right);
        static abstract TResult operator ^(TSelf left, TOther right);
        static abstract TResult operator ~(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface IComparisonOperators<TSelf, TOther>
        : IComparable,
          IComparable<TOther>,
          IEqualityOperators<TSelf, TOther>
        where TSelf : IComparisonOperators<TSelf, TOther>
    {
        static abstract bool operator <(TSelf left, TOther right);
        static abstract bool operator <=(TSelf left, TOther right);
        static abstract bool operator >(TSelf left, TOther right);
        static abstract bool operator >=(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IDecrementOperators<TSelf>
        where TSelf : IDecrementOperators<TSelf>
    {
        static abstract TSelf operator --(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface IDivisionOperators<TSelf, TOther, TResult>
        where TSelf : IDivisionOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator /(TSelf left, TOther right);
        // static abstract checked TResult operator /(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IEqualityOperators<TSelf, TOther> : IEquatable<TOther>
        where TSelf : IEqualityOperators<TSelf, TOther>
    {
        static abstract bool operator ==(TSelf left, TOther right);
        static abstract bool operator !=(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IFloatingPoint<TSelf>
        : ISignedNumber<TSelf>
        where TSelf : IFloatingPoint<TSelf>
    {
        static abstract TSelf E { get; }
        static abstract TSelf Epsilon { get; }
        static abstract TSelf NaN { get; }
        static abstract TSelf NegativeInfinity { get; }
        static abstract TSelf NegativeZero { get; }
        static abstract TSelf Pi { get; }
        static abstract TSelf PositiveInfinity { get; }
        static abstract TSelf Tau { get; }
        static abstract TSelf Acos(TSelf x);
        static abstract TSelf Acosh(TSelf x);
        static abstract TSelf Asin(TSelf x);
        static abstract TSelf Asinh(TSelf x);
        static abstract TSelf Atan(TSelf x);
        static abstract TSelf Atan2(TSelf y, TSelf x);
        static abstract TSelf Atanh(TSelf x);
        static abstract TSelf BitDecrement(TSelf x);
        static abstract TSelf BitIncrement(TSelf x);
        static abstract TSelf Cbrt(TSelf x);
        static abstract TSelf Ceiling(TSelf x);
        static abstract TSelf CopySign(TSelf x, TSelf y);
        static abstract TSelf Cos(TSelf x);
        static abstract TSelf Cosh(TSelf x);
        static abstract TSelf Exp(TSelf x);
        static abstract TSelf Floor(TSelf x);
        static abstract TSelf FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend);
        static abstract TSelf IEEERemainder(TSelf left, TSelf right);
        static abstract TInteger ILogB<TInteger>(TSelf x)
            where TInteger : IBinaryInteger<TInteger>;
        static abstract bool IsFinite(TSelf value);
        static abstract bool IsInfinity(TSelf value);
        static abstract bool IsNaN(TSelf value);
        static abstract bool IsNegative(TSelf value);
        static abstract bool IsNegativeInfinity(TSelf value);
        static abstract bool IsNormal(TSelf value);
        static abstract bool IsPositiveInfinity(TSelf value);
        static abstract bool IsSubnormal(TSelf value);
        static abstract TSelf Log(TSelf x);
        static abstract TSelf Log(TSelf x, TSelf newBase);
        static abstract TSelf Log2(TSelf x);
        static abstract TSelf Log10(TSelf x);
        static abstract TSelf MaxMagnitude(TSelf x, TSelf y);
        static abstract TSelf MinMagnitude(TSelf x, TSelf y);
        static abstract TSelf Pow(TSelf x, TSelf y);
        static abstract TSelf Round(TSelf x);
        static abstract TSelf Round<TInteger>(TSelf x, TInteger digits)
            where TInteger : IBinaryInteger<TInteger>;
        static abstract TSelf Round(TSelf x, MidpointRounding mode);
        static abstract TSelf Round<TInteger>(TSelf x, TInteger digits, MidpointRounding mode)
            where TInteger : IBinaryInteger<TInteger>;
        static abstract TSelf ScaleB<TInteger>(TSelf x, TInteger n)
            where TInteger : IBinaryInteger<TInteger>;
        static abstract TSelf Sin(TSelf x);
        static abstract TSelf Sinh(TSelf x);
        static abstract TSelf Sqrt(TSelf x);
        static abstract TSelf Tan(TSelf x);
        static abstract TSelf Tanh(TSelf x);
        static abstract TSelf Truncate(TSelf x);

        // static abstract TSelf AcosPi(TSelf x);
        // static abstract TSelf AsinPi(TSelf x);
        // static abstract TSelf AtanPi(TSelf x);
        // static abstract TSelf Atan2Pi(TSelf y, TSelf x);
        // static abstract TSelf Compound(TSelf x, TSelf n);
        // static abstract TSelf CosPi(TSelf x);
        // static abstract TSelf ExpM1(TSelf x);
        // static abstract TSelf Exp2(TSelf x);
        // static abstract TSelf Exp2M1(TSelf x);
        // static abstract TSelf Exp10(TSelf x);
        // static abstract TSelf Exp10M1(TSelf x);
        // static abstract TSelf Hypot(TSelf x, TSelf y);
        // static abstract TSelf LogP1(TSelf x);
        // static abstract TSelf Log2P1(TSelf x);
        // static abstract TSelf Log10P1(TSelf x);
        // static abstract TSelf MaxMagnitudeNumber(TSelf x, TSelf y);
        // static abstract TSelf MaxNumber(TSelf x, TSelf y);
        // static abstract TSelf MinMagnitudeNumber(TSelf x, TSelf y);
        // static abstract TSelf MinNumber(TSelf x, TSelf y);
        // static abstract TSelf Root(TSelf x, TSelf n);
        // static abstract TSelf SinPi(TSelf x);
        // static abstract TSelf TanPi(TSelf x);
    }

    [RequiresPreviewFeatures]
    public interface IBinaryFloatingPoint<TSelf>
        : IBinaryNumber<TSelf>,
          IFloatingPoint<TSelf>
        where TSelf : IBinaryFloatingPoint<TSelf>
    {
    }

    [RequiresPreviewFeatures]
    public interface IIncrementOperators<TSelf>
        where TSelf : IIncrementOperators<TSelf>
    {
        static abstract TSelf operator ++(TSelf value);
        // static abstract checked TSelf operator ++(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface IBinaryInteger<TSelf>
        : IBinaryNumber<TSelf>,
          IShiftOperators<TSelf, TSelf>
        where TSelf : IBinaryInteger<TSelf>
    {
        static abstract TSelf LeadingZeroCount(TSelf value);
        static abstract TSelf PopCount(TSelf value);
        static abstract TSelf RotateLeft(TSelf value, TSelf rotateAmount);
        static abstract TSelf RotateRight(TSelf value, TSelf rotateAmount);
        static abstract TSelf TrailingZeroCount(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface IMinMaxValue<TSelf>
        where TSelf : IMinMaxValue<TSelf>
    {
        static abstract TSelf MinValue { get; }
        static abstract TSelf MaxValue { get; }
    }

    [RequiresPreviewFeatures]
    public interface IModulusOperators<TSelf, TOther, TResult>
        where TSelf : IModulusOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator %(TSelf left, TOther right);
        // static abstract checked TResult operator %(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IMultiplicativeIdentity<TSelf, TResult>
        where TSelf : IMultiplicativeIdentity<TSelf, TResult>
    {
        static abstract TResult MultiplicativeIdentity { get; }
    }

    [RequiresPreviewFeatures]
    public interface IMultiplyOperators<TSelf, TOther, TResult>
        where TSelf : IMultiplyOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator *(TSelf left, TOther right);
        // static abstract checked TResult operator *(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface INumber<TSelf>
        : IAdditionOperators<TSelf, TSelf, TSelf>,
          IAdditiveIdentity<TSelf, TSelf>,
          IComparisonOperators<TSelf, TSelf>,   // implies IEquatableOperators<TSelf, TSelf>
          IDecrementOperators<TSelf>,
          IDivisionOperators<TSelf, TSelf, TSelf>,
          IIncrementOperators<TSelf>,
          IModulusOperators<TSelf, TSelf, TSelf>,
          IMultiplicativeIdentity<TSelf, TSelf>,
          IMultiplyOperators<TSelf, TSelf, TSelf>,
          ISpanFormattable,                     // implies IFormattable
          ISpanParseable<TSelf>,                // implies IParseable<TSelf>
          ISubtractionOperators<TSelf, TSelf, TSelf>,
          IUnaryNegationOperators<TSelf, TSelf>,
          IUnaryPlusOperators<TSelf, TSelf>
        where TSelf : INumber<TSelf>
    {
        static abstract TSelf One { get; }
        static abstract TSelf Zero { get; }
        static abstract TSelf Abs(TSelf value);
        static abstract TSelf Clamp(TSelf value, TSelf min, TSelf max);
        static abstract TSelf Create<TOther>(TOther value)
            where TOther : INumber<TOther>;
        static abstract TSelf CreateSaturating<TOther>(TOther value)
            where TOther : INumber<TOther>;
        static abstract TSelf CreateTruncating<TOther>(TOther value)
            where TOther : INumber<TOther>;
        static abstract (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right);
        static abstract TSelf Max(TSelf x, TSelf y);
        static abstract TSelf Min(TSelf x, TSelf y);
        static abstract TSelf Parse(string s, NumberStyles style, IFormatProvider? provider);
        static abstract TSelf Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);
        static abstract TSelf Sign(TSelf value);
        static abstract bool TryCreate<TOther>(TOther value, out TSelf result)
            where TOther : INumber<TOther>;
        static abstract bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out TSelf result);
        static abstract bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out TSelf result);
    }

    [RequiresPreviewFeatures]
    public interface IBinaryNumber<TSelf>
        : IBitwiseOperators<TSelf, TSelf, TSelf>,
          INumber<TSelf>
        where TSelf : IBinaryNumber<TSelf>
    {
        static abstract bool IsPow2(TSelf value);
        static abstract TSelf Log2(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface ISignedNumber<TSelf>
        : INumber<TSelf>
        where TSelf : ISignedNumber<TSelf>
    {
        static abstract TSelf NegativeOne { get; }
    }

    [RequiresPreviewFeatures]
    public interface IUnsignedNumber<TSelf>
        : INumber<TSelf>
        where TSelf : IUnsignedNumber<TSelf>
    {
    }

    [RequiresPreviewFeatures]
    public interface IParseable<TSelf>
        where TSelf : IParseable<TSelf>
    {
        static abstract TSelf Parse(string s, IFormatProvider? provider);
        static abstract bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out TSelf result);
    }

    [RequiresPreviewFeatures]
    public interface IShiftOperators<TSelf, TResult>
        where TSelf : IShiftOperators<TSelf, TResult>
    {
        static abstract TResult operator <<(TSelf value, int shiftAmount); // TODO_GENERIC_MATH: shiftAmount should be TOther
        static abstract TResult operator >>(TSelf value, int shiftAmount); // TODO_GENERIC_MATH: shiftAmount should be TOther
        // static abstract TResult operator >>>(TSelf value, int shiftAmount); // TODO_GENERIC_MATH: shiftAmount should be TOther
    }

    [RequiresPreviewFeatures]
    public interface ISpanParseable<TSelf> : IParseable<TSelf>
        where TSelf : ISpanParseable<TSelf>
    {
        static abstract TSelf Parse(ReadOnlySpan<char> s, IFormatProvider? provider);
        static abstract bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TSelf result);
    }

    [RequiresPreviewFeatures]
    public interface ISubtractionOperators<TSelf, TOther, TResult>
        where TSelf : ISubtractionOperators<TSelf, TOther, TResult>
    {
        static abstract TResult operator -(TSelf left, TOther right);
        // static abstract checked TResult operator -(TSelf left, TOther right);
    }

    [RequiresPreviewFeatures]
    public interface IUnaryNegationOperators<TSelf, TResult>
        where TSelf : IUnaryNegationOperators<TSelf, TResult>
    {
        static abstract TResult operator -(TSelf value);
        // static abstract checked TResult operator -(TSelf value);
    }

    [RequiresPreviewFeatures]
    public interface IUnaryPlusOperators<TSelf, TResult>
        where TSelf : IUnaryPlusOperators<TSelf, TResult>
    {
        static abstract TResult operator +(TSelf value);
        // static abstract checked TResult operator +(TSelf value);
    }
}
