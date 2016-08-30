/// <summary>
/// FIR フィルタ。
/// </summary>
public class IirFilter : IFilter
{
	#region フィールド

	CircularBuffer buf;
	double c;
	double[] a; // 分母係数
	double[] b; // 分子係数

	#endregion
	#region コンストラクタ

	public IirFilter() : this(0, 0, 0) { }

	/// <summary>
	/// 係数を指定して初期化
	/// </summary>
	/// <param name="a0">分母係数 a0</param>
	/// <param name="b0">分母係数 b0</param>
	/// <param name="c">ゲイン</param>
	public IirFilter(double a0, double b0, double c)
		: this(
		new double[] { a0 },
		new double[] { b0 },
		c)
	{
	}

	/// <summary>
	/// 係数を配列で指定して初期化
	/// </summary>
	/// <param name="a">分母係数を格納した配列</param>
	/// <param name="b">分子係数を格納した配列</param>
	/// <param name="c">ゲイン</param>
	public IirFilter(double[] a, double[] b, double c)
	{
		this.a = a;
		this.b = b;
		this.c = c;
		this.buf = new CircularBuffer(a.Length);
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
		double t = this.c * x;
		double y = 0;

		for (int i = 0; i < this.buf.Count; ++i)
		{
			t += this.buf[i] * this.a[i];
			y += this.buf[i] * this.b[i];
		}

		y += t;
		this.buf.Insert(t);
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
		IirFilter f = new IirFilter(this.a, this.b, this.c);
		for (int i = 0; i < this.buf.Count; ++i)
		{
			f.GetValue(this.buf[i]);
		}
		return f;
	}

	#endregion
}
