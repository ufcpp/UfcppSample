public class CircularBuffer : ICloneable
{
	#region フィールド

	double[] buf;
	int length;
	int top;
	int mask;

	#endregion
	#region コンストラクタ

	public CircularBuffer() : this(0) { }

	/// <summary>
	/// バッファ長を指定して初期化
	/// </summary>
	/// <param name="len">バッファ長</param>
	public CircularBuffer(int len)
	{
		this.top = 0;
		this.length = len;

		len = Util.CeilPower2(len);
		this.buf = new double[len];
		this.mask = len - 1;
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
		get { return this.buf[(this.top + n) & this.mask]; }
		set { this.buf[(this.top + n) & this.mask] = value; }
	}

	/// <summary>
	/// 値の挿入
	/// </summary>
	/// <param name="x">挿入したい値</param>
	public void Insert(double x)
	{
		--this.top;
		this.top &= this.mask;
		this.buf[this.top] = x;
	}

	/// <summary>
	/// 要素数
	/// </summary>
	public int Count
	{
		get { return this.length; }
	}

	#endregion
	#region ICloneable メンバ

	public object Clone()
	{
		CircularBuffer cb = new CircularBuffer(this.length);
		cb.buf = (double[])this.buf.Clone();
		cb.top = this.top;
		cb.length = this.length;
		cb.mask = this.mask;
		return cb;
	}

	#endregion
}
