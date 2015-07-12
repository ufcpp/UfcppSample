using System;

namespace SoundLibrary
{
	//! C# 2.0 が正式公開されたら static クラスに。

	/// <summary>
	/// 共通関数群。
	/// </summary>
	public class Util
	{
		#region 正規化・dB⇔リニア値

		/// <summary>
		/// 周波数を正規化。
		/// </summary>
		/// <param name="f">正規化したい周波数</param>
		/// <param name="fs">サンプリング周波数</param>
		/// <returns>正規化角周波数</returns>
		public static double Normalize(double f, double fs)
		{
			return 2 * Math.PI / fs * f;
		}

		/// <summary>
		/// 正規化角周波数を元の周波数に戻す。
		/// </summary>
		/// <param name="w">正規化角周波数</param>
		/// <param name="fs">サンプリング周波数</param>
		/// <returns>元の周波数</returns>
		public static double Denormalize(double w, double fs)
		{
			return fs / (2 * Math.PI) * w;
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

		/// <summary>
		/// dB値 → リニア値に変換。
		/// </summary>
		/// <param name="x">dB値</param>
		/// <returns>リニア値</returns>
		public static double DBToLinear(double x)
		{
			return Math.Pow(10, x/20);
		}

		/// <summary>
		/// リニア値 → dB値に変換。
		/// </summary>
		/// <param name="x">リニア値</param>
		/// <returns>dB値</returns>
		public static double LinearToDB(double x)
		{
			return 20 * Math.Log10(x);
		}

		#endregion
		#region 値のクリッピング

		/// <summary>
		/// 値を short の範囲にクリッピングする。
		/// </summary>
		/// <param name="val">値</param>
		/// <returns>クリッピング後の値</returns>
		/// <remarks>C# 2.0 が正式公開されたら generics 化すると思う。</remarks>
		public static short ClipShort(double val)
		{
			if(val < short.MinValue) val = short.MinValue;
			else if(val > short.MaxValue) val = short.MaxValue;
			return (short)val;
		}

		#endregion
		#region インパルス応答

		/// <summary>
		/// フィルタのインパルス応答を計算する。
		/// </summary>
		/// <param name="f">フィルタ</param>
		/// <param name="len">インパルス応答の長さ</param>
		/// <returns>インパルス応答</returns>
		public static double[] GetImpulseResponse(Filter.IFilter f, int len)
		{
			double[] x = new double[len];
			x[0] = f.GetValue(1);
			for(int i=1; i<len; ++i)
				x[i] = f.GetValue(0);
			return x;
		}

		/// <summary>
		/// フィルタの周波数応答を計算する。
		/// </summary>
		/// <param name="f">フィルタ</param>
		/// <param name="len">インパルス応答の長さ</param>
		/// <returns>周波数応答</returns>
		public static SpectrumAnalysis.Spectrum GetFrequencyResponse(Filter.IFilter f, int len)
		{
			len = SoundLibrary.BitOperation.FloorPower2(len);
			double[] x = GetImpulseResponse(f, len);
			return SpectrumAnalysis.Spectrum.FromTimeSequence(x);
		}

		#endregion
	}
}
