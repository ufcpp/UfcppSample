using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// ホワイトノイズ生成。
	/// (Box-Muller 法を用いて正規乱数を生成。)
	/// </summary>
	public class WhiteNoiseGenerator : IDataGenerator
	{
		double mean;  // 平均
		double sigma; // 標準偏差
		int seed;
		Random rand;

		/// <summary>
		/// 平均0、標準偏差1のホワイトノイズを生成。
		/// </summary>
		public WhiteNoiseGenerator() : this(0, 1, 0){}

		/// <summary>
		/// 平均値と標準偏差を指定してホワイトノイズを生成。
		/// </summary>
		/// <param name="mean">平均値</param>
		/// <param name="sigma">標準偏差</param>
		public WhiteNoiseGenerator(double mean, double sigma, int seed)
		{
			this.mean = mean;
			this.sigma = sigma;
			this.seed = seed;
			this.rand = new Random(seed);
		}

		public double Next()
		{
			double amp = Math.Sqrt(-2 * Math.Log(this.rand.NextDouble()));
			double phase = 2 * Math.PI * this.rand.NextDouble();
			return this.sigma * amp * Math.Sin(phase) + this.mean;
		}

		public void Reset()
		{
			this.rand = new Random(seed);
		}

		public object Clone()
		{
			return new WhiteNoiseGenerator(this.mean, this.sigma, this.seed);
		}
	}
}// namespace
