public class BiquadCoefficient
{
	public double a0;
	public double a1;
	public double a2;
	public double b0;
	public double b1;
	public double b2;

	public BiquadCoefficient(
		double a0, double a1, double a2,
		double b0, double b1, double b2)
	{
		this.a0 = a0;
		this.a1 = a1;
		this.a2 = a2;
		this.b0 = b0;
		this.b1 = b1;
		this.b2 = b2;
	}
}

/// <summary>
/// 2次ずつに分解し、直列接続したタイプの IIR フィルタ。
/// </summary>
public class SerialIirFilter : IFilter
{
	#region 内部クラス

	/// <summary>
	/// 2次 IIR（ゲインなし）。
	/// </summary>
	class Iir2 : System.ICloneable
	{
		double a1, a2, b1, b2;
		double t1, t2;

		public Iir2(
			double a1, double a2, double b1, double b2)
		{
			this.a1 = a1;
			this.a2 = a2;
			this.b1 = b1;
			this.b2 = b2;
		}

		public double GetValue(double x)
		{
			x += this.a1 * this.t1;
			double y = this.b1 * this.t1;
			x += this.a2 * this.t2;
			y += this.b2 * this.t2;
			y += x;
			this.t2 = this.t1;
			this.t1 = x;
			return y;
		}

		public void Clear()
		{
			this.t1 = this.t2 = 0;
		}

		#region ICloneable メンバ

		public object Clone()
		{
			Iir2 f = new Iir2(this.a1, this.a2, this.b1, this.b2);
			f.t1 = this.t1;
			f.t2 = this.t2;
			return f;
		}

		#endregion
	}

	#endregion
	#region フィールド

	double c;
	Iir2[] filters;

	#endregion
	#region コンストラクタ

	public SerialIirFilter(params BiquadCoefficient[] coefs)
	{
		this.filters = new Iir2[coefs.Length];
		this.c = 1;

		for (int i = 0; i < coefs.Length; ++i)
		{
			BiquadCoefficient coef = coefs[i];
			this.c *= coef.b0 / coef.a0;
			this.filters[i] = new Iir2(
				-coef.a1 / coef.a0, -coef.a2 / coef.a0,
				coef.b1 / coef.b0, coef.b2 / coef.b0);
		}
	}

	SerialIirFilter(Iir2[] filters)
	{
	}

	#endregion
	#region IFilter メンバ

	/// <summary>
	/// 各時刻 n で、
	/// t[n] = c * x[n] + Σ a[i - 1] * t[n - i]
	/// y[n] =     t[n] + Σ b[i - 1] * t[n - i]
	/// </summary>
	/// <param name="x">入力</param>
	/// <returns>フィルタ出力</returns>
	public double GetValue(double x)
	{
		double y = x;

		for (int i = 0; i < this.filters.Length; ++i)
		{
			y = this.filters[i].GetValue(y);
		}

		return y;
	}

	public void Clear()
	{
		for (int i = 0; i < this.filters.Length; ++i)
		{
			this.filters[i].Clear();
		}
	}

	#endregion
	#region ICloneable メンバ

	public object Clone()
	{
		return new SerialIirFilter(
			(Iir2[])this.filters.Clone());
	}

	#endregion
}
