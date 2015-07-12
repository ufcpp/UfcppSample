using System;

namespace SpectrumAnalysis
{
	/// <summary>
	/// 複素数クラス。
	/// </summary>
	public struct Complex
	{
		double re; // 実部
		double im; // 虚部

		/// <summary>
		/// 実部を指定して構築。
		/// </summary>
		/// <param name="re">実部</param>
		public Complex(double re) : this(re, 0){}

		/// <summary>
		/// 実部、虚部を指定して構築。
		/// </summary>
		/// <param name="re">実部</param>
		/// <param name="im">虚部</param>
		public Complex(double re, double im)
		{
			this.re = re;
			this.im = im;
		}

		/// <summary>
		/// コピーコンストラクタ。
		/// </summary>
		/// <param name="z">コピー元</param>
		public Complex(Complex z)
		{
			this.re = z.re;
			this.im = z.im;
		}

		/// <summary>
		/// double → Complex の暗黙の方変換。
		/// </summary>
		/// <param name="x">double 値</param>
		/// <returns>x を Complex 化したもの</returns>
		public static implicit operator Complex(double x)
		{
			return new Complex(x);
		}

		/// <summary>
		/// 実部
		/// </summary>
		public double Re
		{
			set{this.re = value;}
			get{return this.re;}
		}

		/// <summary>
		/// 虚部
		/// </summary>
		public double Im
		{
			set{this.im = value;}
			get{return this.im;}
		}

		/// <summary>
		/// パワー(絶対値の二乗)のリニア値。
		/// </summary>
		public double LinearPower
		{
			get{return this.re * this.re + this.im * this.im;}
		}

		/// <summary>
		/// パワー(絶対値の二乗)の dB 値。
		/// </summary>
		public double Power
		{
			get{return 10 * Math.Log10(this.LinearPower);}
		}

		/// <summary>
		/// 絶対値
		/// </summary>
		public double Abs
		{
			get{return Math.Sqrt(this.LinearPower);}
		}

		/// <summary>
		/// 偏角
		/// </summary>
		public double Arg
		{
			get{return Math.Atan2(this.im, this.re);}
		}

		/// <summary>
		/// 共役複素数。
		/// </summary>
		/// <returns>this の共役</returns>
		public Complex Conjugate()
		{
			return new Complex(this.re, -this.im);
		}

		/// <summary>
		/// 逆数。
		/// </summary>
		/// <returns>this の逆数</returns>
		public Complex Invert()
		{
			double pow = this.LinearPower;
			return new Complex(this.re/pow, -this.im/pow);
		}

		/// <summary>
		/// 単項+。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>+a</returns>
		public static Complex operator+ (Complex a)
		{
			return new Complex(a);
		}

		/// <summary>
		/// 単項-。
		/// </summary>
		/// <param name="a">オペランド</param>
		/// <returns>-a</returns>
		public static Complex operator- (Complex a)
		{
			return new Complex(-a.re, -a.im);
		}

		/// <summary>
		/// 加算。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>加算結果</returns>
		public static Complex operator+ (Complex a, Complex b)
		{
			double re = a.re + b.re;
			double im = a.im + b.im;
			return new Complex(re, im);
		}
		public static Complex operator+ (double a, Complex b)
		{
			double re = a + b.re;
			double im = b.im;
			return new Complex(re, im);
		}
		public static Complex operator+ (Complex a, double b)
		{
			double re = a.re + b;
			double im = a.im;
			return new Complex(re, im);
		}

		/// <summary>
		/// 減算。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>減算結果</returns>
		public static Complex operator- (Complex a, Complex b)
		{
			double re = a.re - b.re;
			double im = a.im - b.im;
			return new Complex(re, im);
		}
		public static Complex operator- (double a, Complex b)
		{
			double re = a - b.re;
			double im = -b.im;
			return new Complex(re, im);
		}
		public static Complex operator- (Complex a, double b)
		{
			double re = a.re - b;
			double im = a.im;
			return new Complex(re, im);
		}

		/// <summary>
		/// 乗算。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>乗算結果</returns>
		public static Complex operator* (Complex a, Complex b)
		{
			double re = a.re * b.re - a.im * b.im;
			double im = a.im * b.re + a.re * b.im;
			return new Complex(re, im);
		}
		public static Complex operator* (double a, Complex b)
		{
			double re = a * b.re;
			double im = a * b.im;
			return new Complex(re, im);
		}
		public static Complex operator* (Complex a, double b)
		{
			double re = a.re * b;
			double im = a.im * b;
			return new Complex(re, im);
		}

		/// <summary>
		/// 除算。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>除算結果</returns>
		public static Complex operator/ (Complex a, Complex b)
		{
			return a * b.Invert();
		}
		public static Complex operator/ (double a, Complex b)
		{
			return a * b.Invert();
		}
		public static Complex operator/ (Complex a, double b)
		{
			double re = a.re / b;
			double im = a.im / b;
			return new Complex(re, im);
		}
	}//class Complex
}
