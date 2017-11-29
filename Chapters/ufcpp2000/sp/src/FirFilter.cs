/// <summary>
/// FIR フィルタ。
/// </summary>
public class FirFilter : IFilter
{
	#region フィールド

	CircularBuffer buf;
	double[] a;

	#endregion
	#region コンストラクタ

	public FirFilter() : this(0) { }

	/// <summary>
	/// 係数を指定して初期化
	/// </summary>
	/// <param name="a0">係数 a0</param>
	public FirFilter(double a0)
		: this(new double[] { a0 })
	{
	}

	/// <summary>
	/// 係数を配列で指定して初期化
	/// </summary>
	/// <param name="a">係数を格納した配列</param>
	public FirFilter(params double[] a)
	{
		this.a = a;
		this.buf = new CircularBuffer(a.Length - 1);
	}

	#endregion
	#region IFilter メンバ

	/// <summary>
	/// 各時刻 n で、
	/// y[n] = Σ a[i] * x[n - i]
	/// </summary>
	/// <param name="x">入力</param>
	/// <returns>フィルタ出力</returns>
	public double GetValue(double x)
	{
		double y = x * this.a[0];

		for (int i = 0; i < this.buf.Count; ++i)
		{
			y += this.buf[i] * this.a[i + 1];
		}

		this.buf.Insert(x);
		return y;
	}

	public void Clear()
	{
		for (int n = this.buf.Count; n > 0; --n)
			this.buf.Insert(0);
	}

	#endregion
	#region ICloneable メンバ

	public object Clone()
	{
		FirFilter f = new FirFilter(this.a);
		for (int i = 0; i < this.buf.Count; ++i)
		{
			f.GetValue(this.buf[i]);
		}
		return f;
	}

	#endregion
}
