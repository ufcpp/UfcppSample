using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    /// <summary>
    /// 有理数。
    /// </summary>
    public struct Rational : IComparable<Rational>
    {
        #region フィールド

        int num;
        int denom;

        #endregion
        #region 補助関数

        /// <summary>
        /// 最大公約数を求める。
        /// </summary>
        /// <param name="a">オペランド1。</param>
        /// <param name="b">オペランド2。</param>
        /// <returns></returns>
        public static int Gdc(int a, int b)
        {
            int x = a;
            int y = b;

            if (x < 0) x = -x;
            if (y < 0) y = -y;

            for (; ; )
            {
                int q = x / y;
                int r = x - q * y;
                if (r == 0) break;
                x = y;
                y = r;
            }

            return y;
        }

        /// <summary>
        /// 通分する。
        /// </summary>
        /// <param name="num">分子。</param>
        /// <param name="denom">分母。</param>
        public static void Reduce(ref int num, ref int denom)
        {
            var gdc = Gdc(num, denom);
            num /= gdc;
            denom /= gdc;
        }

        #endregion

        public Rational(int num, int denom)
        {
            Reduce(ref num, ref denom);
            this.num = num;
            this.denom = denom;
        }

        public int Numerator
        {
            get { return num; }
            set { num = value; }
        }

        public int Denominator
        {
            get { return denom; }
            set { denom = value; }
        }

        public static implicit operator Rational(int x)
        {
            return new Rational(x, 1);
        }

        public static explicit operator double(Rational x)
        {
            if (x.num == 0)
                return 0;

            return (double)x.num / (double)x.denom;
        }

        public static Rational operator +(Rational x)
        {
            return new Rational(x.num, x.denom);
        }

        public static Rational operator -(Rational x)
        {
            return new Rational(-x.num, x.denom);
        }

        public static Rational operator +(Rational x, Rational y)
        {
            var num = x.num * y.denom + x.denom * y.num;
            var denom = x.denom * y.denom;

            return new Rational(num, denom);
        }

        public static Rational operator -(Rational x, Rational y)
        {
            var num = x.num * y.denom - x.denom * y.num;
            var denom = x.denom * y.denom;

            return new Rational(num, denom);
        }

        public static Rational operator *(Rational x, Rational y)
        {
            var num = x.num * y.num;
            var denom = x.denom * y.denom;

            return new Rational(num, denom);
        }

        public static Rational operator /(Rational x, Rational y)
        {
            var num = x.num * y.denom;
            var denom = x.denom * y.num;

            return new Rational(num, denom);
        }

        public override string ToString()
        {
            return string.Format("({0}/{1})", this.num, this.denom);
        }

        #region IComparable<Rational> メンバ

        public int CompareTo(Rational other)
        {
            return ((double)this).CompareTo((double)other);
        }

        #endregion
    }

    public static class RationalExtensions
    {
        public static Rational Over(this int x, int y)
        {
            return new Rational(x, y);
        }
    }
}
