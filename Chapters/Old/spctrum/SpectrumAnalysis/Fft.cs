#define CHECK_LENGTH

using System;
using System.Diagnostics;
using FftCpp  = Fft.Fft;
using CFftCpp = Fft.CFft;

namespace SpectrumAnalysis
{
	/// <summary>
	/// Managed C++ で作った Fft クラスのラッパー。
	/// FFT の動作の詳細は fft\fft.cpp の方を参照。
	/// </summary>
	public class Fft
	{
		FftCpp fft;

		public Fft(int length)
		{
			CheckLength(length);

			fft = new FftCpp(length);
		}

		/// <summary>
		/// フーリエ変換を行う。
		/// </summary>
		/// <param name="x">変換したいデータ</param>
		unsafe public void Transform(double[] x)
		{
			fixed(double* px = x)
			{
				fft.Transform(1, px);
			}
		}

		/// <summary>
		/// 逆フーリエ変換を行う。
		/// </summary>
		/// <param name="x">変換したいデータ</param>
		unsafe public void Invert(double[] x)
		{
			fixed(double* px = x)
			{
				fft.Transform(-1, px);
			}
		}

		/// <summary>
		/// len が2のべき乗かどうか調べ、違ったら例外を投げる。
		/// </summary>
		/// <param name="len">調べる長さ</param>
		[Conditional("CHECK_LENGTH")]
		public static void CheckLength(int len)
		{
			if(!IsPower2(len))
				throw new ArgumentException("引数に2のべき乗以外を指定しちゃ駄目");
		}

		/// <summary>
		/// len が2のべき乗かどうか調べる。
		/// </summary>
		/// <param name="len">調べる長さ</param>
		/// <returns>2のべき乗ならtrue</returns>
		static bool IsPower2(int len)
		{
			if(len < 0)
				return false;

			while(len != 0)
			{
				if(len%2 != 0)
					return len/2 == 0;
				len /= 2;
			}

			return false;
		}
	}//class Fft

	/// <summary>
	/// Managed C++ で作った Fft クラスのラッパー。
	/// FFT の動作の詳細は fft\fft.cpp の方を参照。
	/// </summary>
	public class CFft
	{
		CFftCpp fft;

		public CFft(int length)
		{
			Fft.CheckLength(length);

			fft = new CFftCpp(length);
		}

		/// <summary>
		/// フーリエ変換を行う。
		/// </summary>
		/// <param name="x">変換したいデータ</param>
		unsafe public void Transform(double[] x)
		{
			fixed(double* px = x)
			{
				fft.Transform(1, px);
			}
		}

		/// <summary>
		/// 逆フーリエ変換を行う。
		/// </summary>
		/// <param name="x">変換したいデータ</param>
		unsafe public void Invert(double[] x)
		{
			fixed(double* px = x)
			{
				fft.Transform(-1, px);
			}
		}
	}//class CFft
}//namespace Spectrum
