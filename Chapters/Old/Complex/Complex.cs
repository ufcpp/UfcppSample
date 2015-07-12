using System;

namespace MyMath
{
	/// <summary>
	/// 複素数型の抽象基底クラス
	/// </summary>
	public abstract class Complex : ICloneable
	{
		#region ICloneable メンバ

		public abstract object Clone();
		public abstract override bool Equals(object o);
		public abstract bool Equals(double x);
		public abstract override int GetHashCode();

		#endregion
		#region abstract プロパティ

		/// <summary>
		/// 実部
		/// </summary>
		public abstract double Re{set; get;}

		/// <summary>
		/// 虚部
		/// </summary>
		public abstract double Im{set; get;}

		/// <summary>
		/// 絶対値
		/// </summary>
		public abstract double Abs{set; get;}

		/// <summary>
		/// 偏角
		/// </summary>
		public abstract double Arg{set; get;}

		#endregion
		#region abstract メソッド

		/// <summary>
		/// 絶対値の二乗を返す
		/// </summary>
		public abstract double Norm();

		/// <summary>
		/// this を符号反転させたものを返す
		/// </summary>
		public abstract Complex Negate();

		/// <summary>
		/// this の逆数を返す
		/// </summary>
		public abstract Complex Invert();

		/// <summary>
		/// this の共役複素数を返す
		/// </summary>
		public abstract Complex Conjugate();

		/// <summary>
		/// this に z を加えた値を返す
		/// (thisの値は変化させない)
		/// </summary>
		public abstract Complex Add(Complex z);
		public abstract Complex Add(double x);

		/// <summary>
		/// this から z を引いた値を返す
		/// (thisの値は変化させない)
		/// </summary>
		public abstract Complex Sub(Complex z);
		public abstract Complex Sub(double x);

		/// <summary>
		/// this に z を掛けた値を返す
		/// (thisの値は変化させない)
		/// </summary>
		public abstract Complex Mul(Complex z);
		public abstract Complex Mul(double x);

		/// <summary>
		/// this を z で割った値を返す
		/// (thisの値は変化させない)
		/// </summary>
		public abstract Complex Div(Complex z);
		public abstract Complex Div(double x);

		#endregion
		#region object メンバ

		/// <summary>
		/// 文字列化
		/// </summary>
		public override string ToString()
		{
			if(this.Im > 0)
				return this.Re + "+i" + this.Im;
			if(this.Im < 0)
				return this.Re + "-i" + (-this.Im);
			return this.Re.ToString();
		}

		#endregion
		#region operator

		#region 単項演算子

		/// <summary>
		/// 単項+
		/// </summary>
		static public Complex operator+ (Complex z)
		{
			return (Complex)z.Clone();
		}

		/// <summary>
		/// 単項-
		/// </summary>
		static public Complex operator- (Complex z)
		{
			return z.Negate();
		}

		/// <summary>
		/// 共役演算子
		/// </summary>
		static public Complex operator~ (Complex z)
		{
			return z.Conjugate();
		}

		#endregion
		#region 2項演算子

		/// <summary>
		/// 加算
		/// </summary>
		/// <returns>z+w</returns>
		static public Complex operator+ (Complex z, Complex w)
		{
			return z.Add(w);
		}
		static public Complex operator+ (Complex z, double x)
		{
			return z.Add(x);
		}
		static public Complex operator+ (double x, Complex z)
		{
			return z.Add(x);
		}

		/// <summary>
		/// 減算
		/// </summary>
		/// <returns>z-w</returns>
		static public Complex operator- (Complex z, Complex w)
		{
			return z.Sub(w);
		}
		static public Complex operator- (Complex z, double x)
		{
			return z.Sub(x);
		}
		static public Complex operator- (double x, Complex z)
		{
			return z.Negate().Add(x);
		}

		/// <summary>
		/// 乗算
		/// </summary>
		/// <returns>z*w</returns>
		static public Complex operator* (Complex z, Complex w)
		{
			return z.Mul(w);
		}
		static public Complex operator* (Complex z, double x)
		{
			return z.Mul(x);
		}
		static public Complex operator* (double x, Complex z)
		{
			return z.Mul(x);
		}

		/// <summary>
		/// 除算
		/// </summary>
		/// <returns>z/w</returns>
		static public Complex operator/ (Complex z, Complex w)
		{
			return z.Div(w);
		}
		static public Complex operator/ (Complex z, double x)
		{
			return z.Div(x);
		}
		static public Complex operator/ (double x, Complex z)
		{
			return z.Invert().Mul(x);
		}

		#endregion
		#region 比較演算子

		/// <summary>
		/// z と w の値の比較
		/// </summary>
		static public bool operator== (Complex z, Complex w)
		{
			return z.Equals(w);
		}
		static public bool operator== (Complex z, double x)
		{
			return z.Equals(x);
		}
		static public bool operator== (double x, Complex z)
		{
			return z.Equals(x);
		}

		/// <summary>
		/// z と w の値の比較
		/// </summary>
		static public bool operator!= (Complex z, Complex w)
		{
			return !z.Equals(w);
		}
		static public bool operator!= (Complex z, double x)
		{
			return !z.Equals(x);
		}
		static public bool operator!= (double x, Complex z)
		{
			return !z.Equals(x);
		}

		#endregion

		#endregion
		#region 静的メソッド

		static protected double Norm_(double x, double y)
		{
			return x*x + y*y;
		}

		static protected double Abs_(double x, double y)
		{
			return Math.Sqrt(Norm_(x, y));
		}

		static protected double Arg_(double x, double y)
		{
			return Math.Atan2(y, x);
		}

		#endregion
	}//class Complex
}