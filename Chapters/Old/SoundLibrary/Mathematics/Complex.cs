using System;

namespace SoundLibrary.Mathematics
{
	/// <summary>
	/// 複素数。
	/// </summary>
	public struct Complex
	{
		double re; // 実部
		double im; // 虚部

		#region コンストラクタ・構築用メソッド

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
		/// 偏角を指定して絶対値1の複素数を作成する。
		/// </summary>
		/// <param name="arg">偏角</param>
		/// <returns>指定した偏角を持つ絶対値1の複素数</returns>
		public static Complex FromArg(double arg)
		{
			double re = Math.Cos(arg);
			double im = Math.Sin(arg);
			return new Complex(re, im);
		}

		/// <summary>
		/// 絶対と偏角を指定して複素数を作成する。
		/// </summary>
		/// <param name="abs">絶対値</param>
		/// <param name="arg">偏角</param>
		/// <returns>指定した絶対値と偏角を持つ複素数</returns>
		public static Complex FromPolar(double abs, double arg)
		{
			double re = abs * Math.Cos(arg);
			double im = abs * Math.Sin(arg);
			return new Complex(re, im);
		}

		/// <summary>
		/// パワーのdB値と偏角を指定して複素数を作成する。
		/// </summary>
		/// <param name="power">パワーのdB値</param>
		/// <param name="arg">偏角</param>
		/// <returns>指定した絶対値と偏角を持つ複素数</returns>
		public static Complex FromPowerPolar(double power, double arg)
		{
			return Complex.FromPolar(Util.DBToLinear(power), arg);
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

		#endregion
		#region 実部・虚部

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
		/// 実部と虚部を設定する。
		/// </summary>
		/// <param name="re">実部</param>
		/// <param name="im">虚部</param>
		public void SetCartesian(double re, double im)
		{
			this.re = re;
			this.im = im;
		}

		#endregion
		#region 絶対値・偏角

		/// <summary>
		/// 絶対と偏角を設定する。
		/// </summary>
		/// <param name="abs">絶対値</param>
		/// <param name="arg">偏角</param>
		public void SetPolar(double abs, double arg)
		{
			this.re = abs * Math.Cos(arg);
			this.im = abs * Math.Sin(arg);
		}

		/// <summary>
		/// 絶対(パワーのdB値で指定)と偏角を設定する。
		/// </summary>
		/// <param name="power">パワーのdB値</param>
		/// <param name="arg">偏角</param>
		public void SetPowerPolar(double power, double arg)
		{
			this.SetPolar(Util.DBToLinear(power), arg);
		}

		/// <summary>
		/// 偏角を設定する(絶対値は1)。
		/// </summary>
		/// <param name="arg">偏角</param>
		public void SetArg(double arg)
		{
			this.re = Math.Cos(arg);
			this.im = Math.Sin(arg);
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

		#endregion
		#region 演算子・変換メソッド

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

		/// <summary>
		/// 等値判定。
		/// </summary>
		/// <param name="a">左オペランド</param>
		/// <param name="b">右オペランド</param>
		/// <returns>判定結果</returns>
		public static bool operator== (Complex a, Complex b)
		{
			return a.re == b.re && a.im == b.im;
		}
		public static bool operator== (Complex a, double b)
		{
			return a.re == b && a.im == 0;
		}
		public static bool operator== (double a, Complex b)
		{
			return a == b.re && 0 == b.im;
		}

		public static bool operator!= (Complex a, Complex b)
		{
			return a.re != b.re || a.im != b.im;
		}
		public static bool operator!= (Complex a, double b)
		{
			return a.re != b || a.im != 0;
		}
		public static bool operator!= (double a, Complex b)
		{
			return a != b.re || 0 != b.im;
		}

		#endregion
		#region object

		public override bool Equals(object obj)
		{
			Complex c = (Complex)obj;
			return this.re.Equals(c.re) && this.im.Equals(c.im);
		}

		public override int GetHashCode()
		{
			return this.re.GetHashCode() ^ this.im.GetHashCode();
		}

		#endregion
		#region string 文字列化
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.re, this.Im);
		}
		#endregion
	}//class Complex
}
