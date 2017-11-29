using System;

namespace MyMath
{
	/// <summary>
	/// 複素数クラスの実装2
	/// 絶対値と偏角をメンバに持つ
	/// 加減算は激遅で、乗除算は高速
	/// </summary>
	public class PolarComplex : Complex
	{
		#region 定数

		private const double TwoPI = 2 * Math.PI;
		static public readonly PolarComplex I = new PolarComplex(0, Math.PI);

		#endregion
		#region 初期化

		public PolarComplex(double r, double t)
		{
			this.abs = r;
			this.arg = t % TwoPI;
		}

		public PolarComplex(Complex z) : this(z.Abs, z.Arg){}
		public PolarComplex(double r) : this(r, 0){}
		public PolarComplex() : this(0, 0){}
		static public implicit operator PolarComplex (double x){return new PolarComplex(x);}

		/// <summary>
		/// 直行座標からコンストラクト
		/// </summary>
		/// <param name="x">実部</param>
		/// <param name="y">虚部</param>
		static PolarComplex FromCartesian(double x, double y)
		{
			return new PolarComplex(Abs_(x, y), Arg_(x, y));
		}

		#endregion
		#region ICloneable メンバ

		public override object Clone()
		{
			return new PolarComplex(this.abs, this.arg);
		}

		public override bool Equals(object o)
		{
			if(!(o is Complex))
				return false;

			Complex z = (Complex)o;
			return (this.abs == z.Abs) || (this.arg == z.Arg);
		}
		public override bool Equals(double x)
		{
			return (this.arg == 0) || (this.abs == x);
		}

		public override int GetHashCode()
		{
			return this.abs.GetHashCode() ^ this.arg.GetHashCode();
		}

		#endregion
		#region override プロパティ

		public override double Re
		{
			set
			{
				double im = this.Im;
				this.abs = Abs_(value, im);
				this.arg = Arg_(value, im);
			}
			get
			{
				return this.abs * Math.Cos(this.arg);
			}
		}

		public override double Im
		{
			set
			{
				double re = this.Re;
				this.abs = Abs_(re, value);
				this.arg = Arg_(re, value);
			}
			get
			{
				return this.abs * Math.Sin(this.arg);
			}
		}

		public override double Abs
		{
			set{this.abs = value;}
			get{return this.abs;}
		}

		public override double Arg
		{
			set{this.arg = value;}
			get{return this.arg;}
		}

		#endregion
		#region override メソッド

		public override Complex Negate()
		{
			return new PolarComplex(this.abs, this.arg + Math.PI);
		}

		public override Complex Invert()
		{
			return new PolarComplex(1.0 / this.abs, -this.arg);
		}

		public override Complex Conjugate()
		{
			return new PolarComplex(this.abs, -this.arg);
		}

		public override Complex Add(Complex z)
		{
			return FromCartesian(this.Re + z.Re, this.Im + z.Im);
		}
		public override Complex Add(double x)
		{
			return FromCartesian(this.Re + x, this.Im);
		}

		public override Complex Sub(Complex z)
		{
			return FromCartesian(this.Re - z.Re, this.Im - z.Im);
		}
		public override Complex Sub(double x)
		{
			return FromCartesian(this.Re - x, this.Im);
		}

		public override Complex Mul(Complex z)
		{
			return new PolarComplex(this.abs * z.Abs, this.arg + z.Arg);
		}
		public override Complex Mul(double x)
		{
			return new PolarComplex(this.abs * x, this.arg);
		}

		public override Complex Div(Complex z)
		{
			return new PolarComplex(this.abs / z.Abs, this.arg - z.Arg);
		}
		public override Complex Div(double x)
		{
			return new PolarComplex(this.abs / x, this.arg);
		}

		public override double Norm()
		{
			return this.abs * this.abs;
		}

		#endregion
		#region フィールド

		private double abs; // 絶対値
		private double arg; // 偏角

		#endregion
	}//class PolarComplex
}
