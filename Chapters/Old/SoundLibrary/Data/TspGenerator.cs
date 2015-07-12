using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// TSP êMçÜ
	/// </summary>
	public class TspGenerator : IDataGenerator
	{
		int i = 0;
		int taps;
		double amp;
		double phase;

		public TspGenerator() : this(512, short.MaxValue, 300){}

		public TspGenerator(int taps, double gain, double m)
		{
			this.taps = taps;
			this.amp = 2 * gain / taps;
			this.phase = 4 * m * Math.PI / (taps * taps);
		}

		TspGenerator(TspGenerator x)
		{
			this.taps  = x.taps;
			this.amp   = x.amp;
			this.phase = x.phase;
		}

		public double Next()
		{
			double x = this.amp / 2;
			for(int k=1; k<taps; ++k)
			{
				double phase = this.phase  * k * k;
				x += this.amp * Math.Cos(Math.PI / this.taps * k * this.i + phase);
			}
			++this.i;
			return x;
		}

		public void Reset()
		{
			this.i = 0;
		}

		public object Clone()
		{
			return new TspGenerator(this);
		}
	}
}//namespace Data
