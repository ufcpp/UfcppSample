using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// フーリエ級数を使ってデータを生成する。
	/// </summary>
	public class FourierSeriesGenerator : IDataGenerator
	{
		const int N = 512;
		int i = 0;
		double[] gain;
		double[] phase;

		public FourierSeriesGenerator(double[] gain, double[] phase)
		{
			this.gain = gain;
			this.phase = phase;
		}

		public double Next()
		{
			double x = 0;
			for(int k=0; k<N; ++k)
				x += this.gain[k] * Math.Cos(Math.PI / N * k * this.i + this.phase[k]);
			++this.i;
			return x;
		}

		public void Reset()
		{
			this.i = 0;
		}

		public object Clone()
		{
			return new FourierSeriesGenerator(this.gain, this.phase);
		}
	}
}//namespace Data
