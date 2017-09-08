using System;
using System.Diagnostics;

namespace Grisu3DoubleConversion
{
    /// <summary>
    /// https://github.com/google/double-conversion/blob/master/double-conversion/fast-dtoa.cc
    /// </summary>
    public static unsafe class DoubleConversion
    {
        private const int kMinimalTargetExponent = -60;
        private const int kMaximalTargetExponent = -32;

        private static bool RoundWeed(
            Span<byte> buffer,
            int length,
            ulong distance_too_high_w,
            ulong unsafe_interval,
            ulong rest,
            ulong ten_kappa,
            ulong unit)
        {
            ulong small_distance = distance_too_high_w - unit;
            ulong big_distance = distance_too_high_w + unit;
            Debug.Assert(rest <= unsafe_interval);
            while (rest < small_distance &&  // Negated condition 1
                unsafe_interval - rest >= ten_kappa &&  // Negated condition 2
                (rest + ten_kappa < small_distance ||  // buffer{-1} > w_high
                small_distance - rest >= rest + ten_kappa - small_distance))
            {
                buffer[length - 1]--;
                rest += ten_kappa;
            }

            if (rest < big_distance &&
                unsafe_interval - rest >= ten_kappa &&
                (rest + ten_kappa < big_distance || big_distance - rest > rest + ten_kappa - big_distance))
            {
                return false;
            }

            return (2 * unit <= rest) && (rest <= unsafe_interval - 4 * unit);
        }

        private static readonly uint[] kSmallPowersOfTen = { 0, 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        private static void BiggestPowerTen(
            uint number,
            int number_bits,
            out uint power,
            out int exponent_plus_one)
        {
            Debug.Assert(number < (1u << (number_bits + 1)));
            int exponent_plus_one_guess = ((number_bits + 1) * 1233 >> 12);
            exponent_plus_one_guess++;
            if (number < kSmallPowersOfTen[exponent_plus_one_guess])
            {
                exponent_plus_one_guess--;
            }
            power = kSmallPowersOfTen[exponent_plus_one_guess];
            exponent_plus_one = exponent_plus_one_guess;
        }

        private static bool DigitGen(
            DiyFp low,
            DiyFp w,
            DiyFp high,
            Span<byte> buffer,
            out int length,
            out int kappa)
        {
            Debug.Assert(low.E == w.E && w.E == high.E);
            Debug.Assert(low.F + 1 <= high.F - 1);
            Debug.Assert(kMinimalTargetExponent <= w.E && w.E <= kMaximalTargetExponent);

            ulong unit = 1;
            DiyFp too_low = new DiyFp(low.F - unit, low.E);
            DiyFp too_high = new DiyFp(high.F + unit, high.E);
            DiyFp unsafe_interval = too_high - too_low;
            DiyFp one = new DiyFp(1UL << -w.E, w.E);
            uint integrals = unchecked((uint)(too_high.F >> -one.E));
            ulong fractionals = too_high.F & (one.F - 1);
            BiggestPowerTen(integrals, DiyFp.SignificandSize - (-one.E),
                out var divisor, out var divisor_exponent_plus_one);
            kappa = divisor_exponent_plus_one;
            length = 0;
            while (kappa > 0)
            {
                int digit = unchecked((int)(integrals / divisor));
                Debug.Assert(digit <= 9);
                buffer[length] = (byte)('0' + digit);
                length++;
                integrals %= divisor;
                kappa--;
                ulong rest =
                    ((ulong)(integrals) << -one.E) + fractionals;
                if (rest < unsafe_interval.F)
                {
                    return RoundWeed(buffer, length, (too_high - w).F,
                                     unsafe_interval.F, rest,
                                     (ulong)(divisor) << -one.E, unit);
                }
                divisor /= 10;
            }

            Debug.Assert(one.E >= -60);
            Debug.Assert(fractionals < one.F);
            Debug.Assert(0xFFFFFFFF_FFFFFFFFUL / 10 >= one.F);
            for (; ; )
            {
                fractionals *= 10;
                unit *= 10;
                unsafe_interval.F = (unsafe_interval.F * 10);
                int digit = unchecked((int)(fractionals >> -one.E));
                Debug.Assert(digit <= 9);
                buffer[length] = (byte)('0' + digit);
                (length)++;
                fractionals &= one.F - 1;  // Modulo by one.
                (kappa)--;
                if (fractionals < unsafe_interval.F)
                {
                    return RoundWeed(buffer, length, (too_high - w).F * unit,
                                     unsafe_interval.F, fractionals, one.F, unit);
                }
            }
        }

        public static bool ToString(
            double v,
            bool isSinglePrecision,
            Span<byte> buffer,
            out int length,
            out int decimal_exponent)
        {
            DiyFp w = new DoubleView(v).AsNormalizedDiyFp();

            DiyFp boundary_minus, boundary_plus;
            if (!isSinglePrecision)
            {
                new DoubleView(v).NormalizedBoundaries(out boundary_minus, out boundary_plus);
            }
            else
            {
                new SingleView((float)v).NormalizedBoundaries(out boundary_minus, out boundary_plus);
            }
            Debug.Assert(boundary_plus.E == w.E);
            int ten_mk_minimal_binary_exponent = kMinimalTargetExponent - (w.E + DiyFp.SignificandSize);
            int ten_mk_maximal_binary_exponent = kMaximalTargetExponent - (w.E + DiyFp.SignificandSize);
            PowersOfTenCache.GetCachedPowerForBinaryExponentRange(
                ten_mk_minimal_binary_exponent,
                ten_mk_maximal_binary_exponent,
                out var ten_mk, out var mk);
            Debug.Assert((kMinimalTargetExponent <= w.E + ten_mk.E +
                    DiyFp.SignificandSize) &&
                   (kMaximalTargetExponent >= w.E + ten_mk.E +
                    DiyFp.SignificandSize));

            DiyFp scaled_w = (w * ten_mk);
            Debug.Assert(scaled_w.E ==
                   boundary_plus.E + ten_mk.E + DiyFp.SignificandSize);

            DiyFp scaled_boundary_minus = (boundary_minus * ten_mk);
            DiyFp scaled_boundary_plus = (boundary_plus * ten_mk);

            bool result = DigitGen(scaled_boundary_minus, scaled_w, scaled_boundary_plus,
                                   buffer, out length, out var kappa);
            decimal_exponent = -mk + kappa;
            return result;
        }

        public static bool ToString(float v, out NumberStringBuffer buffer)
        {
            if (v == 0)
            {
                buffer = NumberStringBuffer.Zero;
                return true;
            }
            else if(float.IsPositiveInfinity(v))
            {
                buffer = NumberStringBuffer.PositiveInfinity;
                return false;
            }
            else if (float.IsNegativeInfinity(v))
            {
                buffer = NumberStringBuffer.NegativeInfinity;
                return false;
            }
            else if (float.IsNaN(v))
            {
                buffer = NumberStringBuffer.NaN;
                return false;
            }

            buffer = default;
            buffer.IsSinglePrecision = true;
            if (v < 0)
            {
                buffer.IsNegative = true;
                v = -v;
            }

            fixed (byte* p = buffer.Digits)
            {
                var result = ToString(v, true, new Span<byte>(p, NumberStringBuffer.MaxDigits), out var len, out var exp);
                buffer.Length = (byte)len;
                buffer.DecimalExponent = (short)exp;
                return result;
            }
        }

        public static bool ToString(double v, out NumberStringBuffer buffer)
        {
            if (v == 0)
            {
                buffer = NumberStringBuffer.Zero;
                return true;
            }
            else if (double.IsPositiveInfinity(v))
            {
                buffer = NumberStringBuffer.PositiveInfinity;
                return false;
            }
            else if (double.IsNegativeInfinity(v))
            {
                buffer = NumberStringBuffer.NegativeInfinity;
                return false;
            }
            else if (double.IsNaN(v))
            {
                buffer = NumberStringBuffer.NaN;
                return false;
            }

            buffer = default;
            if (v < 0)
            {
                buffer.IsNegative = true;
                v = -v;
            }

            fixed (byte* p = buffer.Digits)
            {
                var result = ToString(v, false, new Span<byte>(p, NumberStringBuffer.MaxDigits), out var len, out var exp);
                buffer.Length = (byte)len;
                buffer.DecimalExponent = (short)exp;
                return result;
            }
        }
    }
}
