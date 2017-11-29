using System;

namespace SoundLibrary.Filter.Delay
{
	/// <summary>
	/// 遅延フィルタのインタフェース。
	/// </summary>
	public interface IDelay : IFilter
	{
		/// <summary>
		/// 遅延時間[サンプル]。
		/// </summary>
		double DelayTime
		{
			get;
			set;
		}

		/// <summary>
		/// DelayTime サンプル遅れの値を取り出すだけ。
		/// </summary>
		/// <returns>取り出した値</returns>
		double GetValue();

		/// <summary>
		/// 内部バッファの途中の値を取り出す。
		/// ! リバーブ用。
		/// </summary>
		/// <param name="n">値を取り出したい位置</param>
		/// <returns>取り出した値</returns>
		double GetBufferValue(int n);

		/// <summary>
		/// 値のプッシュ。
		/// </summary>
		/// <param name="x">プッシュしたい値</param>
		void Push(double x);
	}
}
