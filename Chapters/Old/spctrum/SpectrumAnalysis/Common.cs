using System;

namespace SpectrumAnalysis
{
	/// <summary>
	/// Spectrum 名前空間内の共通関数群
	/// </summary>
	class Common
	{
		/// <summary>
		/// 周波数を正規化。
		/// </summary>
		/// <param name="w">正規化したい周波数</param>
		/// <param name="ws">サンプリング周波数</param>
		/// <returns></returns>
		public static double Normalize(double w, double ws)
		{
			return 2 * Math.PI / ws * w;
		}

		/// <summary>
		/// 周波数を正規化。
		/// サンプリング周波数は 48000Hz。
		/// </summary>
		/// <param name="w">正規化したい周波数</param>
		/// <returns></returns>
		public static double Normalize(double w)
		{
			return Normalize(w, 48000);
		}

		/// <summary>
		/// 複素数の絶対値(パワーのdB値)を求める。
		/// </summary>
		/// <param name="re">実部</param>
		/// <param name="im">虚部</param>
		/// <returns>パワーのdB値</returns>
		public static double Amp(double re, double im)
		{
			return 10 * Math.Log10(re*re + im*im);
		}

		/// <summary>
		/// 絶対値(パワーのdB値)を求める。
		/// </summary>
		/// <param name="re">リニア値</param>
		/// <returns>パワーのdB値</returns>
		public static double Amp(double re)
		{
			return 20 * Math.Log10(Math.Abs(re));
		}

		/// <summary>
		/// 複素数の偏角を求める。
		/// </summary>
		/// <param name="re">実部</param>
		/// <param name="im">虚部</param>
		/// <returns>偏角</returns>
		public static double Phase(double re, double im)
		{
			return Math.Atan2(im, re);
		}
	}//class Common
}