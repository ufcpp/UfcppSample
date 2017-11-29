using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// ランプ関数生成
	/// </summary>
	public class RampGenerator : IDataGenerator
	{
		double gain;
		ushort i = 0;
		public RampGenerator() : this(1) {}
		public RampGenerator(double gain){this.gain = gain;}

		public double Next()
		{
			double x = (ushort)(this.gain * this.i);
			++this.i;
			return x;
		}

		public void Reset()
		{
			this.i = 0;
		}

		public object Clone()
		{
			return new RampGenerator(this.gain);
		}
	}
}//namespace Data
