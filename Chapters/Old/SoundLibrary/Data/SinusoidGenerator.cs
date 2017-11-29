using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// 正弦波生成
	/// </summary>
	public class SinusoidGenerator : IDataGenerator
	{
		int i = 0;
		double w;
		double gain;
		double phase;

		public SinusoidGenerator() : this(0) {}
		public SinusoidGenerator(double phase) : this((ushort)short.MaxValue, phase) {}
		public SinusoidGenerator(double gain, double phase) : this(Math.PI / 24000 * 1000, gain, phase) {}

		/// <summary>
		/// 周波数、振幅、初期位相を指定。
		/// </summary>
		/// <param name="w">周波数(2πで正規化)</param>
		/// <param name="gain">振幅(リニア値)</param>
		/// <param name="phase">初期位相(rad)</param>
		public SinusoidGenerator(double w, double gain, double phase)
		{
			this.w = w;
			this.gain = gain;
			this.phase = phase;
		}

		public double Next()
		{
			double x = this.gain * Math.Sin(this.w * this.i + phase);
			++this.i;
			return x;
		}

		public void Reset()
		{
			this.i = 0;
		}

		public object Clone()
		{
			return new SinusoidGenerator(this.w, this.gain, this.phase);
		}
	}
}//namespace Data
