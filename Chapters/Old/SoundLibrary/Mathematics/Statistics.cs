using System;

namespace SoundLibrary.Mathematics
{
	/// <summary>
	/// 統計処理関数群を定義。
	/// </summary>
	public class Statistics
	{
		/// <summary>
		/// 配列 x の和。
		/// </summary>
		public static double Sum(double[] x)
		{
			double sum = 0;
			for(int i=0; i<x.Length; ++i)
				sum += x[i];
			return sum;
		}

		/// <summary>
		/// 配列 x の自乗和。
		/// </summary>
		public static double SquareSum(double[] x)
		{
			double sum = 0;
			for(int i=0; i<x.Length; ++i)
				sum += x[i] * x[i];
			return sum;
		}

		/// <summary>
		/// 配列 x と y の積和。
		/// </summary>
		public static double Mac(double[] x, double[] y)
		{
			double mac = 0;
			for(int i=0; i<x.Length; ++i)
				mac += x[i] * y[i];
			return mac;
		}

		/// <summary>
		/// 配列 x の平均値。
		/// </summary>
		public static double Average(double[] x)
		{
			return Sum(x) / x.Length;
		}

		/// <summary>
		/// 配列 x の分散。
		/// </summary>
		static double Variance(double[] x)
		{
			double var = Average(x);
			var = var * var;
			var = SquareSum(x) / x.Length - var;
			return var;
		}

		/// <summary>
		/// 配列 x と y の共分散。
		/// </summary>
		public static double Covariance(double[] x, double[] y)
		{
			double var = Mac(x, y) / x.Length - Average(x) * Average(y);
			return var;
		}
	}
}
