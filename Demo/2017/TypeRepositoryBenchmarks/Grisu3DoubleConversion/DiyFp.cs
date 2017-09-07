using System.Diagnostics;

namespace Grisu3DoubleConversion
{
    /// <summary>
    /// https://github.com/google/double-conversion/blob/master/double-conversion/diy-fp.cc
    /// </summary>
    public struct DiyFp
    {
        public ulong F;
        public int E;

        public const ulong Uint64MSB = 0x80000000_00000000;
        public const int SignificandSize = 64;

        public DiyFp(ulong significand, int exponent)
        {
            F = significand;
            E = exponent;
        }

        // this = this - other.
        // The exponents of both numbers must be the same and the significand of this
        // must be bigger than the significand of other.
        // The result will not be normalized.
        void Subtract(DiyFp other)
        {
            Debug.Assert(E == other.E);
            Debug.Assert(F >= other.F);
            F -= other.F;
        }

        // Returns a - b.
        // The exponents of both numbers must be the same and this must be bigger
        // than other. The result will not be normalized.
        public static DiyFp operator -(DiyFp a, DiyFp b)
        {
            DiyFp result = a;
            result.Subtract(b);
            return result;
        }


        // this = this * other.
        void Multiply(DiyFp other)
        {
            // Simply "emulates" a 128 bit multiplication.
            // However: the resulting number only contains 64 bits. The least
            // significant 64 bits are only used for rounding the most significant 64
            // bits.
            const ulong kM32 = 0xFFFFFFFFU;
            ulong a = F >> 32;
            ulong b = F & kM32;
            ulong c = other.F >> 32;
            ulong d = other.F & kM32;
            ulong ac = a * c;
            ulong bc = b * c;
            ulong ad = a * d;
            ulong bd = b * d;
            ulong tmp = (bd >> 32) + (ad & kM32) + (bc & kM32);
            // By adding 1U << 31 to tmp we round the final result.
            // Halfway cases will be round up.
            tmp += 1U << 31;
            ulong result_f = ac + (ad >> 32) + (bc >> 32) + (tmp >> 32);
            E += other.E + 64;
            F = result_f;
        }

        // returns a * b;
        public static DiyFp operator *(DiyFp a, DiyFp b)
        {
            DiyFp result = a;
            result.Multiply(b);
            return result;
        }

        public void Normalize()
        {
            Debug.Assert(F != 0);
            ulong significand = F;
            int exponent = E;

            // This method is mainly called for normalizing boundaries. In general
            // boundaries need to be shifted by 10 bits. We thus optimize for this case.
            const ulong k10MSBits = 0xFFC00000_00000000;
            while ((significand & k10MSBits) == 0)
            {
                significand <<= 10;
                exponent -= 10;
            }
            while ((significand & Uint64MSB) == 0)
            {
                significand <<= 1;
                exponent--;
            }
            F = significand;
            E = exponent;
        }

        public static DiyFp Normalize(DiyFp a)
        {
            DiyFp result = a;
            result.Normalize();
            return result;
        }
    }
}
