/// <summary>
/// 音声処理用フィルタインターフェース。
/// </summary>
public interface IFilter : ICloneable
{
	/// <summary>
	/// フィルタリングを行い、その結果を返す。
	/// </summary>
	/// <param name="x">フィルタ入力。</param>
	/// <returns>フィルタ出力。</returns>
	double GetValue(double x);

	/// <summary>
	/// フィルタの内部状態をクリアする。
	/// </summary>
	void Clear();
}
