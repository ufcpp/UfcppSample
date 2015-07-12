using System;

namespace SoundLibrary.Mathematics.Continuous
{
	using Type = System.Double;

	/// <summary>
	/// 連続関数を表すクラス。
	/// </summary>
	public abstract class Function
	{
		/// <summary>
		/// 関数値 f(t) を計算。
		/// </summary>
		public abstract Type this[double t]
		{
			get;
		}
	}
}
