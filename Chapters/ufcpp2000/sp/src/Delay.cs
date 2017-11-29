/// <summary>
/// 遅延器。
/// </summary>
public class Delay : IFilter
{
	#region フィールド

	CircularBuffer buf;

	#endregion
	#region コンストラクタ

	public Delay() : this(1) { }

	/// <summary>
	/// 倍率を指定して初期化
	/// </summary>
	/// <param name="delaytime">遅延時間[sample数]</param>
	public Delay(int delaytime)
	{
		this.buf = new CircularBuffer(delaytime);
	}

	#endregion
	#region IFilter メンバ

	public double GetValue(double x)
	{
		double y = this.buf[this.buf.Count - 1];
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
		Delay d = new Delay(this.buf.Count);
		for (int i = 0; i < this.buf.Count; ++i)
		{
			d.GetValue(this.buf[i]);
		}
		return d;
	}

	#endregion
}
