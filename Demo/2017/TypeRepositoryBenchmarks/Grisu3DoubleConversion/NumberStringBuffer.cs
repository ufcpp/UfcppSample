using System;
using System.Text;

namespace Grisu3DoubleConversion
{
    public unsafe struct NumberStringBuffer
    {
        public const int MaxDigits = 17; // IEEE 754 double presicion floating point

        public short DecimalExponent;
        public byte Length;

        public bool IsNegative;
        public bool IsSinglePrecision;
        public bool IsInfinity;
        public bool IsNaN;

        public static NumberStringBuffer PositiveInfinity => new NumberStringBuffer { IsInfinity = true };
        public static NumberStringBuffer NegativeInfinity => new NumberStringBuffer { IsNegative = true, IsInfinity = true };
        public static NumberStringBuffer NaN => new NumberStringBuffer { IsNaN = true };
        public static NumberStringBuffer Zero
        {
            get
            {
                var buf = default(NumberStringBuffer);
                buf.Digits[0] = (byte)'0';
                buf.Length = 1;
                return buf;
            }
        }

        public fixed byte Digits[MaxDigits];

        public byte[] GetUtf8Bytes()
        {
            var buffer = stackalloc byte[MaxDigits + 7]; // '-' "Digits"(max = 17) '.' 'E' '-' "Exponent"(max = 3)
            var length = Format(buffer);

            var a = new byte[length];
            fixed(byte* pa = a) Buffer.MemoryCopy(buffer, pa, length, length);
            return a;
        }

        public override string ToString()
        {
            var buffer = stackalloc byte[MaxDigits + 7]; // '-' "Digits"(max = 17) '.' 'E' '-' "Exponent"(max = 3)
            var length = Format(buffer);
            return Encoding.UTF8.GetString(buffer, length);
        }

        private int Format(byte* buffer)
        {
            var exp = Length + DecimalExponent;

            if (exp > 0 && exp <= (IsSinglePrecision ? 7 : 15))
            {
                return FormatPositiveExp(buffer);
            }
            else if (exp <= 0 && exp > -4)
            {
                return FormatNegativeExp(buffer);
            }

            return FormatScientific(buffer);
        }

        private int FormatPositiveExp(byte* buffer)
        {
            var pb = buffer;

            if (IsNegative)
            {
                *(pb++) = (byte)'-';
            }

            fixed (byte* fd = Digits)
            {
                var pd = fd;

                int posLength = Length;
                if (DecimalExponent < 0) posLength += DecimalExponent;

                for (int i = 0; i < posLength; i++) *(pb++) = *(pd++);
                for (int i = 0; i < DecimalExponent; i++) *(pb++) = (byte)'0';

                if (DecimalExponent < 0)
                {
                    *(pb++) = (byte)'.';
                    for (int i = DecimalExponent; i < 0; i++) *(pb++) = *(pd++);
                }
            }

            int length = (int)(pb - buffer);
            return length;
        }

        private int FormatNegativeExp(byte* buffer)
        {
            var pb = buffer;

            if (IsNegative)
            {
                *(pb++) = (byte)'-';
            }

            *(pb++) = (byte)'0';
            *(pb++) = (byte)'.';

            var exp = Length + DecimalExponent;

            for (int i = 0; i < -exp; i++)
            {
                *(pb++) = (byte)'0';
            }

            fixed (byte* fd = Digits)
            {
                var pd = fd;

                for (int i = 0; i < Length; i++)
                {
                    *(pb++) = *(pd++);
                }
            }

            int length = (int)(pb - buffer);
            return length;
        }

        private int FormatScientific(byte* buffer)
        {
            var pb = buffer;

            if (IsNegative)
            {
                *(pb++) = (byte)'-';
            }

            if(IsInfinity)
            {
                *(pb++) = 226;
                *(pb++) = 136;
                *(pb++) = 158;
                goto END;
            }
            else if (IsNaN)
            {
                *(pb++) = 78;
                *(pb++) = 97;
                *(pb++) = 78;
                goto END;
            }

            fixed (byte* fd = Digits)
            {
                var pd = fd;
                var last = fd + Length;

                *(pb++) = *(pd++);

                if (Length > 1)
                {
                    *(pb++) = (byte)'.';

                    while (pd != last) *(pb++) = *(pd++);
                }

                var exp = Length + DecimalExponent - 1;
                if (exp != 0)
                {
                    *(pb++) = (byte)'E';

                    if (exp < 0)
                    {
                        *(pb++) = (byte)'-';
                        exp = -exp;
                    }
                    else
                    {
                        *(pb++) = (byte)'+';
                    }

                    if (exp >= 100)
                    {
                        *(pb++) = (byte)(exp / 100 + '0');
                        exp %= 100;
                    }

                    *(pb++) = (byte)(exp / 10 + '0');
                    exp %= 10;
                    *(pb++) = (byte)(exp + '0');
                }
            }

            END:
            int length = (int)(pb - buffer);
            return length;
        }
    }
}
