using System;

using Ellip = SoundLibrary.Mathematics.Elliptic;

namespace SoundLibrary.Mathematics.Expression
{
	using CoefType   = System.Double;
	using DomainType = System.Double;
	using ValueType  = System.Double;

	/// <summary>
	/// 有理式。
	/// </summary>
	public class Rational
	{
		#region フィールド

		Polynomial num;
		Polynomial denom;

		#endregion
		#region 初期化

		/// <summary>
		/// 0次数で初期化。
		/// </summary>
		public Rational() : this(0) {}

		/// <summary>
		/// 分子多項式の次数を指定して初期化。
		/// </summary>
		/// <param name="nOrder">分子多項式の次数</param>
		public Rational(int nOrder) : this(nOrder, 0) {}

		/// <summary>
		/// 次数を指定して初期化。
		/// </summary>
		/// <param name="nOrder">分子多項式の次数</param>
		/// <param name="dOrder">分母多項式の次数</param>
		public Rational(int nOrder, int dOrder) : this(new Polynomial(nOrder), new Polynomial(dOrder)) {}

		/// <summary>
		/// 分子/分母多項式を指定して初期化。
		/// </summary>
		/// <param name="num">分子多項式</param>
		/// <param name="denom">分母多項式</param>
		public Rational(Polynomial num, Polynomial denom)
		{
			this.num   = num;
			this.denom = denom;
		}

		#endregion
		#region 値の計算

		/// <summary>
		/// f(x) を計算。
		/// </summary>
		/// <param name="x">x</param>
		/// <returns>f(x)</returns>
		public ValueType Value(DomainType x)
		{
			return this.num.Value(x) / this.denom.Value(x);
		}

		#endregion
		#region 分子/分母多項式の取得

		/// <summary>
		/// 分子多項式。
		/// </summary>
		public Polynomial Numerator
		{
			get{return this.num;}
			set{this.num = value;}
		}

		/// <summary>
		/// 分母多項式。
		/// </summary>
		public Polynomial Denominator
		{
			get{return this.denom;}
			set{this.denom = value;}
		}

		#endregion
		#region operator

		/// <summary>
		/// 単項+。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <returns>+f(x)</returns>
		public static Rational operator+ (Rational f)
		{
			return new Rational(+f.num, +f.denom);
		}

		/// <summary>
		/// 単項-。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <returns>-f(x)</returns>
		public static Rational operator- (Rational f)
		{
			return new Rational(-f.num, +f.denom);
		}

		/// <summary>
		/// 多項式同士の加算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) + g(x)</returns>
		public static Rational operator+ (Rational f, Rational g)
		{
			Polynomial num = f.num * g.denom + f.denom * g.denom;
			Polynomial denom = f.denom * g.denom;

			return new Rational(num, denom);
		}

		/// <summary>
		/// 多項式同士の減算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) - g(x)</returns>
		public static Rational operator- (Rational f, Rational g)
		{
			Polynomial num = f.num * g.denom - f.denom * g.denom;
			Polynomial denom = f.denom * g.denom;

			return new Rational(num, denom);
		}

		/// <summary>
		/// 多項式同士の乗算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) * g(x)</returns>
		public static Rational operator* (Rational f, Rational g)
		{
			Polynomial num = f.num * g.num;
			Polynomial denom = f.denom * g.denom;

			return new Rational(num, denom);
		}

		/// <summary>
		/// 多項式同士の除算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) / g(x)</returns>
		public static Rational operator/ (Rational f, Rational g)
		{
			Polynomial num = f.num * g.denom;
			Polynomial denom = f.denom * g.num;

			return new Rational(num, denom);
		}

		#endregion
		#region 特殊な多項式を取得

		#region チェビシェフ有理式

		/// <summary>
		/// チェビシェフ有理式(elliptic rational)を計算する。
		/// </summary>
		/// <param name="n">次数</param>
		/// <param name="l">x > 1 における極小値</param>
		/// <returns>次数 n のチェビシェフ有理式</returns>
		public static Rational Elliptic(int n, double l)
		{
			double m1 = 1 / (l * l);
			double m1p = 1 - m1;

			double Kk1  = Ellip.K(m1);
			double Kk1p = Ellip.K(m1p);

			double m    = Ellip.InverseQ(Math.Exp(-Math.PI * Kk1p / (n * Kk1)));
			double Kk   = Ellip.K(m);

			Polynomial num   = Polynomial.X(0, 1);
			Polynomial denom = Polynomial.X(0, 1);

			Rational r = new Rational(Polynomial.X(0, 1), Polynomial.X(0, 1));
			double g = 1;

			for(int i=n-1; i>0; i-=2)
			{
				double u = Kk * (double)i / n;
				double sn = Ellip.Sn(u, m);
				double w = sn * sn;

				g *= (m*w - 1) / (1/w - 1);
				num   *= Polynomial.X(2,	1/w) - 1;
				denom *= Polynomial.X(2, m*w) - 1;
			}

			if((n & 1) == 1)
			{
				num *= Polynomial.X(1);
			}

			num *= g;

			return new Rational(num, denom);
		}

		#endregion

		#endregion
	}
}
