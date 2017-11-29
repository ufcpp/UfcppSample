public class CircularBuffer : ICloneable
{
	#region フィールド

	double[] buf;

	#endregion
	#region コンストラクタ

	public CircularBuffer() : this(0) { }

	/// <summary>
	/// バッファ長を指定して初期化
	/// </summary>
	/// <param name="len">バッファ長</param>
	public CircularBuffer(int len)
	{
		this.buf = new double[len];
	}

	#endregion
	#region 値の挿入・取得

	/// <summary>
	/// n サンプル前の値の取得
	/// </summary>
	/// <param name="n">何サンプル前の値を読み書きするか</param>
	/// <returns>n サンプル前の値</returns>
	public double this[int n]
	{
		get { return this.buf[n]; }
		set { this.buf[n] = value; }
	}

	/// <summary>
	/// 値の挿入
	/// </summary>
	/// <param name="x">挿入したい値</param>
	public void Insert(double x)
	{
		for (int n = this.buf.Length - 1; n > 0; --n)
		{
			this.buf[n] = this.buf[n - 1];
		}
		this.buf[0] = x;
	}

	/// <summary>
	/// 要素数
	/// </summary>
	public int Count
	{
		get { return this.buf.Length; }
	}

	#endregion
	#region ICloneable メンバ

	public object Clone()
	{
		CircularBuffer cb = new CircularBuffer(this.Count);
		cb.buf = (double[])this.buf.Clone();
		return cb;
	}

	#endregion
}
