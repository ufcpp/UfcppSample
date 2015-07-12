using System;

//! todo
/*
 * 差、単項-、畳み込み (operator)
 * 微分・積分
 * http://www5.airnet.ne.jp/tomy/cpro/sslib9.htm
 * 最小二乗近似
 * Chebyshev 近似
 * 
 * 有理式も作ろう。
 * Fourier Series とかも。
 */

namespace SoundLibrary.Mathematics.Expression
{
	using CoefType   = System.Double;
	using DomainType = System.Double;
	using ValueType  = System.Double;

	/// <summary>
	/// 多項式。
	/// </summary>
	public class Polynomial : ICloneable
	{
		#region フィールド

		/// <summary>
		/// coef[n] … n次の係数。
		/// </summary>
		CoefType[] coef;

		#endregion
		#region 初期化

		public Polynomial() : this(0) {}

		/// <summary>
		/// 次数を指定して初期化。
		/// </summary>
		/// <param name="order">多項式の次数</param>
		public Polynomial(int order) : this(new CoefType[order + 1]) {}

		/// <summary>
		/// 係数配列を指定して初期化。
		/// </summary>
		/// <param name="coef">係数配列</param>
		public Polynomial(params CoefType[] coef)
		{
			this.coef = coef;
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
			int n=coef.Length-1;
			ValueType y = this.coef[n];

			while(n > 0)
			{
				y *= x;
				--n;
				y += this.coef[n];
			}

			return y;
		}

		#endregion
		#region 係数の取得

		public CoefType[] Coef
		{
			get{return this.coef;}
		}

		#endregion
		#region operator

		/// <summary>
		/// 単項+。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <returns>+f(x)</returns>
		public static Polynomial operator+ (Polynomial f)
		{
			return f.Clone();
		}

		/// <summary>
		/// 多項式同士の加算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) + g(x)</returns>
		public static Polynomial operator+ (Polynomial f, Polynomial g)
		{
			CoefType[] a, b, c;
			Select(f.coef, g.coef, out a, out b);
			c = new CoefType[a.Length];

			int n = 0;
			for(; n<b.Length; ++n) c[n] = a[n] + b[n];
			for(; n<a.Length; ++n) c[n] = a[n];

			return new Polynomial(c);
		}

		/// <summary>
		/// 多項式同士の減算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) - g(x)</returns>
		public static Polynomial operator- (Polynomial f, Polynomial g)
		{
			CoefType[] c;

			if(f.coef.Length > g.coef.Length)
			{
				c = new CoefType[f.coef.Length];

				int n = 0;
				for(; n<g.coef.Length; ++n) c[n] = f.coef[n] - g.coef[n];
				for(; n<f.coef.Length; ++n) c[n] = f.coef[n];
			}
			else
			{
				c = new CoefType[g.coef.Length];

				int n = 0;
				for(; n<f.coef.Length; ++n) c[n] = f.coef[n] - g.coef[n];
				for(; n<g.coef.Length; ++n) c[n] = -g.coef[n];
			}

			return new Polynomial(c);
		}

		/// <summary>
		/// -f
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <returns>-f(x)</returns>
		public static Polynomial operator-(Polynomial f)
		{
			CoefType[] c;
			c = new CoefType[f.coef.Length];
			for(int n=0; n<f.coef.Length; ++n) c[n] = -f.coef[n];
			return new Polynomial(c);
		}

		/// <summary>
		/// 多項式同士の乗算。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="g">g(x)</param>
		/// <returns>f(x) × g(x)</returns>
		public static Polynomial operator* (Polynomial f, Polynomial g)
		{
			CoefType[] c = Convolute(f.coef, g.coef);
			return new Polynomial(c);
		}

		/// <summary>
		/// 多項式÷係数体。
		/// </summary>
		/// <param name="f">f(x)</param>
		/// <param name="a">a</param>
		/// <returns>f(x) ÷ a</returns>
		public static Polynomial operator/ (Polynomial f, CoefType a)
		{
			CoefType[] c = (CoefType[])f.coef.Clone();
			for(int n=0; n<c.Length; ++n) c[n] /= a;
			return new Polynomial(c);
		}

		/// <summary>
		/// 係数→多項式のキャスト。
		/// </summary>
		/// <param name="a">係数</param>
		/// <returns>Polynominal</returns>
		public static implicit operator Polynomial (CoefType a)
		{
			return new Polynomial(a);
		}

		#endregion
		#region 特殊な多項式を取得

		#region x のべき乗

		/// <summary>
		/// x の n 乗を返す。
		/// </summary>
		/// <param name="n">指数</param>
		/// <returns>x の n 乗</returns>
		public static Polynomial X(int n)
		{
			return Polynomial.X(n, 1);
		}

		/// <summary>
		/// a x^n を返す。
		/// </summary>
		/// <param name="n">指数</param>
		/// <param name="a">係数</param>
		/// <returns>x の n 乗</returns>
		public static Polynomial X(int n, CoefType a)
		{
			CoefType[] c = new CoefType[n + 1];
			for(int i=0; i<n; ++i) c[i] = 0;
			c[n] = a;

			return new Polynomial(c);
		}

		#endregion
		#region チェビシェフ多項式

		/// <summary>
		/// チェビシェフ多項式を計算する。
		/// </summary>
		/// <param name="n">次数</param>
		/// <returns>次数 n のチェビシェフ多項式</returns>
		public static Polynomial Chebyshev(int n)
		{
			if(n == 0)
				return Polynomial.X(0, 1);
			else if(n == 1)
				return Polynomial.X(1, 1);
			
			return Polynomial.X(1, 2) * Polynomial.Chebyshev(n - 1) - Polynomial.Chebyshev(n - 2);
		}

		#endregion
		#region ラグランジュ補間

		public static Polynomial Lagrange(DomainType[] x, DomainType[] y)
		{
			if(x.Length != y.Length)
				throw new System.ArgumentException("x と y の次数は等しくなければいけません。");

			int len = x.Length;
			Polynomial p = (Polynomial)(CoefType)0.0;
			Polynomial X = Polynomial.X(1);

			for(int i=0; i<len; ++i)
			{
				Polynomial q = (Polynomial)(CoefType)y[i];

				for(int j=0; j<len; ++j)
				{
					if(i == j) continue;

					Polynomial temp = (X - (CoefType)x[j]);
					temp /= (x[i] - x[j]);
					q *= (X - (CoefType)x[j]) / (x[i] - (CoefType)x[j]);
				}
				p += q;
			}

			return p;
		}

		#endregion

		#endregion
		#region static 関数

		/// <summary>
		/// x と y のうち、長い方の配列を a に、短い方を b に格納。
		/// </summary>
		static void Select(CoefType[] x, CoefType[] y, out CoefType[] a, out CoefType[] b)
		{
			if(x.Length > y.Length)
			{
				a = x;
				b = y;
			}
			else
			{
				a = y;
				b = x;
			}
		}

		/// <summary>
		/// 配列の畳込み積を計算する。
		/// </summary>
		/// <param name="x">配列1</param>
		/// <param name="y">配列2</param>
		/// <returns>x * y</returns>
		static CoefType[] Convolute(CoefType[] x, CoefType[] y)
		{
			CoefType[] a, b, c;

			Select(x, y, out a, out b);

			c = new CoefType[a.Length + b.Length - 1];
			for(int k=0; k<c.Length; ++k) c[k] = 0;

			int i=0;
			for(; i<b.Length; ++i)
				for(int k=0, l=i; k<=i; ++k, --l)
					c[i] += a[k] * b[l];
			for(; i<a.Length; ++i)
				for(int k=i, l=0; l<b.Length; --k, ++l)
					c[i] += a[k] * b[l];
			for(; i<c.Length; ++i)
				for(int l=b.Length-1, k=i-l; k<a.Length; ++k, --l)
					c[i] += a[k] * b[l];

			return c;
		}

		#endregion
		#region object

		public override bool Equals(object obj)
		{
			Polynomial p = obj as Polynomial;

			if(p == null)
			{
				return false;
			}

			if(this.coef.Length != p.coef.Length)
				return false;

			for(int i=0; i<this.coef.Length; ++i)
			{
				if(this.coef[i] != p.coef[i])
					return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			int code = 2376498;

			for(int i=0; i<this.coef.Length; ++i)
			{
				code <<= 2;
				code ^= this.coef[i].GetHashCode();
			}

			return (int)code;
		}

		#endregion
		#region ICloneable メンバ

		public Polynomial Clone()
		{
			return new Polynomial((CoefType[])this.coef.Clone());
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion
	}
}
