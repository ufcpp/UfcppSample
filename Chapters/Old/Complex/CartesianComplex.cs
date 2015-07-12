using System;

namespace MyMath
{
	/// <summary>
	/// 複素数クラスの実装1
	/// 実部と虚部をメンバに持つ
	/// 加減算は高速で、乗除算は低速
	/// </summary>
	public class CartesianComplex : Complex
	{
		#region 定数

		static public readonly CartesianComplex I = new CartesianComplex(0, 1);

		#endregion
		#region 初期化

		public CartesianComplex(double x, double y)
		{
			this.re = x;
			this.im = y;
		}

		public CartesianComplex(Complex z) : this(z.Re, z.Im){}
		public CartesianComplex(double x) : this(x, 0){}
		public CartesianComplex() : this(0, 0){}
		static public implicit operator CartesianComplex (double x){return new CartesianComplex(x);}

		/// <summary>
		/// 極座標からコンストラクト
		/// </summary>
		/// <param name="r">絶対値</param>
		/// <param name="theta">偏角</param>
		static CartesianComplex FromPolar(double r, double theta)
		{
			return new CartesianComplex(r * Math.Cos(theta), r * Math.Sin(theta));
		}

		#endregion
		#region ICloneable メンバ

		public override object Clone()
		{
			return new CartesianComplex(this.re, this.im);
		}

		public override bool Equals(object o)
		{
			if(!(o is Complex))
				return false;

			Complex z = (Complex)o;
			return (this.re == z.Re) && (this.im == z.Im);
		}
		public override bool Equals(double x)
		{
			return (this.im == 0) && (this.re == x);
		}

		public override int GetHashCode()
		{
			return this.re.GetHashCode() ^ this.im.GetHashCode();
		}

		#endregion
		#region override プロパティ

		public override double Re
		{
			set{this.re = value;}
			get{return this.re;}
		}

		public override double Im
		{
			set{this.im = value;}
			get{return this.im;}
		}

		public override double Abs
		{
			set
			{
				double abs = this.Abs;
				this.re *= value / abs;
				this.im *= value / abs;
			}
			get
			{
				return Abs_(this.re, this.im);
			}
		}

		public override double Arg
		{
			set
			{
				double abs = this.Abs;
				this.re = abs * Math.Cos(value);
				this.im = abs * Math.Sin(value);
			}
			get
			{
				return Arg_(this.re, this.im);
			}
		}

		#endregion
		#region override メソッド

		public override Complex Negate()
		{
			return new CartesianComplex(-this.re, -this.im);
		}

		public override Complex Conjugate()
		{
			return new CartesianComplex(this.re, -this.im);
		}

		public override Complex Invert()
		{
			double norm = this.Norm();
			return new CartesianComplex(this.re / norm, -this.im / norm);
		}

		public override Complex Add(Complex z)
		{
			return new CartesianComplex(this.re + z.Re, this.im + z.Im);
		}
		public override Complex Add(double x)
		{
			return new CartesianComplex(this.re + x, this.im);
		}

		public override Complex Sub(Complex z)
		{
			return new CartesianComplex(this.re - z.Re, this.im - z.Im);
		}
		public override Complex Sub(double x)
		{
			return new CartesianComplex(this.re - x, this.im);
		}

		public override Complex Mul(Complex z)
		{
			return new CartesianComplex(this.re * z.Re - this.im * z.Im, this.re * z.Im + this.im * z.Re);
		}
		public override Complex Mul(double x)
		{
			return new CartesianComplex(this.re * x, this.im * x);
		}

		public override Complex Div(Complex z)
		{
			CartesianComplex w = new CartesianComplex(this.re * z.Re + this.im * z.Im, this.im * z.Re - this.re * z.Im);
			w.re /= z.Norm();
			w.im /= z.Norm();
			return w;
		}
		public override Complex Div(double x)
		{
			return new CartesianComplex(this.re / x, this.im / x);
		}

		public override double Norm()
		{
			return Norm_(this.re, this.im);
		}

		#endregion
		#region フィールド

		private double re; // 実部
		private double im; // 虚部

		#endregion
	}//class CartesianComplex
}
