using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericOperator
{
    /// <summary>
    /// Operator を使った例ということで、ジェネリックな複素数型を実装。
    /// </summary>
    /// <typeparam name="T">実部・虚部の型</typeparam>
    public struct Complex<T>
        where T: IComparable<T>
    {
        #region フィールド

        private T re;
        private T im;

        #endregion
        #region 初期化

        /// <summary>
        /// 実部・虚部を与えて初期化。
        /// </summary>
        /// <param name="re">実部の値。</param>
        /// <param name="im">虚部の値。</param>
        public Complex(T re, T im)
        {
            this.re = re;
            this.im = im;
        }

        #endregion
        #region プロパティ

        /// <summary>
        /// 実部。
        /// </summary>
        public T Re
        {
            get { return re; }
            set { re = value; }
        }

        /// <summary>
        /// 虚部。
        /// </summary>
        public T Im
        {
            get { return im; }
            set { im = value; }
        }

        #endregion
        #region タイピング量を減らすための簡単なラッパー

        static T Add(T x, T y) { return Operator<T>.Add(x, y); }
        static T Sub(T x, T y) { return Operator<T>.Subtract(x, y); }
        static T Mul(T x, T y) { return Operator<T>.Multiply(x, y); }
        static T Div(T x, T y) { return Operator<T>.Divide(x, y); }
        static T Neg(T x) { return Operator<T>.Negate(x); }
        static T Acc(T x, T y, T z, T w) { return Operator<T>.ProductSum(x, y, z, w); }
        static T Det(T x, T y, T z, T w) { return Operator<T>.ProductDifference(x, y, z, w); }
        static T Norm(T x, T y) { return Operator<T>.ProductSum(x, y, x, y); }

        #endregion
        #region 加減乗除

        public static Complex<T> operator +(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(Add(x.re, y.re), Add(x.im, y.im));
        }
        public static Complex<T> operator +(T x, Complex<T> y)
        {
            return new Complex<T>(Add(x, y.re), y.im);
        }
        public static Complex<T> operator +(Complex<T> x, T y)
        {
            return new Complex<T>(Add(x.re, y), x.im);
        }

        public static Complex<T> operator -(Complex<T> x)
        {
            return new Complex<T>(Neg(x.re), Neg(x.im));
        }

        public static Complex<T> operator -(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(Sub(x.re, y.re), Sub(x.im, y.im));
        }
        public static Complex<T> operator -(T x, Complex<T> y)
        {
            return new Complex<T>(Sub(x, y.re), y.im);
        }
        public static Complex<T> operator -(Complex<T> x, T y)
        {
            return new Complex<T>(Sub(x.re, y), x.im);
        }

        public static Complex<T> operator *(Complex<T> x, Complex<T> y)
        {
            return new Complex<T>(
                Det(x.re, y.re, x.im, y.im),
                Acc(x.re, y.im, x.im, y.re));
        }
        public static Complex<T> operator *(T x, Complex<T> y)
        {
            return new Complex<T>(Mul(x, y.re), Mul(x, y.im));
        }
        public static Complex<T> operator *(Complex<T> x, T y)
        {
            return new Complex<T>(Mul(x.re, y), Mul(x.im, y));
        }

        /// <summary>
        /// 逆数を求める。
        /// </summary>
        /// <returns>逆数。</returns>
        public Complex<T> Inverse()
        {
            T norm = Norm(this.re, this.im);
            T re = Div(this.re, norm);
            T im = Neg(Div(this.im, norm));
            return new Complex<T>(re, im);
        }

        public static Complex<T> operator /(Complex<T> x, Complex<T> y)
        {
            return x * y.Inverse();
        }
        public static Complex<T> operator /(T x, Complex<T> y)
        {
            return x * y.Inverse();
        }
        public static Complex<T> operator /(Complex<T> x, T y)
        {
            return new Complex<T>(Div(x.re, y), Div(x.im, y));
        }

        #endregion
        #region 文字列化

        public override string ToString()
        {
            if (this.im.CompareTo(default(T)) < 0)
            {
                return string.Format("{0} - i{1}", this.re, Neg(this.im));
            }
            else
            {
                return string.Format("{0} + i{1}", this.re, this.im);
            }
        }

        #endregion
    }

    /// <summary>
    /// Complex に付随する拡張メソッド。
    /// </summary>
    public static class ComplexExtensions
    {
        /// <summary>
        /// 5.I() みたいな書き方で純虚数を作れるようするための拡張メソッド。
        /// </summary>
        /// <typeparam name="T">虚部の型。</typeparam>
        /// <param name="x">虚部の値。</param>
        /// <returns>純虚数 ix</returns>
        public static Complex<T> I<T>(this T x)
            where T : IComparable<T>
        {
            return new Complex<T>(default(T), x);
        }

        /// <summary>
        /// 3 .I(4) みたいな書き方で複素数 3 + i4 を作れるようにするための拡張メソッド。
        /// </summary>
        /// <typeparam name="T">実部・虚部の型。</typeparam>
        /// <param name="x">実部。</param>
        /// <param name="y">虚部。</param>
        /// <returns>複素数 x + iy</returns>
        public static Complex<T> I<T>(this T x, T y)
            where T : IComparable<T>
        {
            return new Complex<T>(x, y);
        }
    }
}
