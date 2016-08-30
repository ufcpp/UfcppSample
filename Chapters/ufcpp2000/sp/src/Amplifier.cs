/// <summary>
/// 増幅器。
/// </summary>
public class Amplifier : IFilter
{
	#region フィールド

	double amp; // 倍率

	#endregion
	#region コンストラクタ

	public Amplifier() : this(0) { }

	/// <summary>
	/// 倍率を指定して初期化
	/// </summary>
	/// <param name="amp">倍率</param>
	public Amplifier(double amp)
	{
		this.amp = amp;
	}

	#endregion
	#region プロパティ

	/// <summary>
	/// 倍率
	/// </summary>
	public double Amplitude
	{
		get { return this.amp; }
		set { this.amp = value; }
	}

	#endregion
	#region IFilter メンバ

	public double GetValue(double x)
	{
		return this.amp * x;
	}

	public void Clear()
	{
	}

	#endregion
	#region ICloneable メンバ

	public object Clone()
	{
		return new Amplifier(this.amp);
	}

	#endregion
}
