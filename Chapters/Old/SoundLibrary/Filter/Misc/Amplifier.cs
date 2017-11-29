using System;

namespace SoundLibrary.Filter.Misc
{
	/// <summary>
	/// 増幅器
	/// </summary>
	public class Amplifier : IFilter
	{
		double gain;

		public Amplifier(double gain)
		{
			this.gain = gain;
		}

		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			return this.gain * x;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
		}

		public object Clone()
		{
			return new Amplifier(this.gain);
		}

		/// <summary>
		/// 増幅率
		/// </summary>
		public double Gain
		{
			get{return this.gain;}
			set{this.gain = value;}
		}
	}//class Amplifier
}
