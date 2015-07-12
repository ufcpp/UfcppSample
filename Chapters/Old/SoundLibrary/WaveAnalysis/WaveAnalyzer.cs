using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave ファイル解析用クラス。
	/// </summary>
	public class WaveAnalyzer
	{
		WaveData data = new WaveTime(); // データ音

		/// <summary>
		/// データ Wave の取得
		/// </summary>
		public WaveData Data
		{
			get{return this.data;}
			set{this.data = value;}
		}

		/// <summary>
		/// 配列のピーク値を求める。
		/// </summary>
		/// <param name="x">配列</param>
		/// <returns>ピーク値</returns>
		static double GetPeekValue(double[] x)
		{
			if(x == null) return 0;

			double peek = 0;
			for(int i=0; i<x.Length; ++i)
			{
				if(Math.Abs(x[i]) > peek) peek = Math.Abs(x[i]);
			}
			return peek;
		}

		/// <summary>
		/// 無音区間の長さを求める。
		/// 
		/// </summary>
		/// <param name="x">配列</param>
		/// <param name="threshold">閾値</param>
		/// <returns>区間長</returns>
		static int GetSilentLength(double[] x, double threshold)
		{
			if(x == null) return int.MaxValue;

			int skip;
			for(skip=0; skip<x.Length; ++skip)
			{
				if(Math.Abs(x[skip]) >= threshold) break;
			}
			return skip;
		}

		/// <summary>
		/// 配列をコピー
		/// </summary>
		/// <param name="src">コピー元</param>
		/// <param name="index">コピー開始位置</param>
		/// <param name="length">コピーする長さ</param>
		/// <returns></returns>
		static double[] CopyArray(double[] src, int index, int length)
		{
			double[] dst = new double[length];
			for(int i=0; i<length; ++i) dst[i] = src[i + index];
			return dst;
		}

		/// <summary>
		/// 配列をコピー
		/// コピー後の配列の長さの方が長い場合、後ろを0詰め
		/// </summary>
		/// <param name="src">コピー元</param>
		/// <param name="index">コピー開始位置</param>
		/// <param name="length">コピーする長さ</param>
		/// <param name="dstLength">コピー後の配列の長さ</param>
		/// <returns></returns>
		static double[] CopyArray(double[] src, int index, int length, int dstLength)
		{
			if(length > dstLength)
				return CopyArray(src, index, length);

			double[] dst = new double[dstLength];
			int i;
			for(i=0; i<length; ++i) dst[i] = src[i + index];
			for(; i<dstLength; ++i) dst[i] = 0;
			return dst;
		}

		/// <summary>
		/// ファイルから Wave データを読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <returns>読み出したデータ</returns>
		static WaveData Read(string filename, int skip, int length)
		{
			WaveReader reader = null;
			FormatHeader header;
			double[] l;
			double[] r;

			using(reader = new WaveReader(filename))
			{
				header = reader.Header;
				reader.Skip(skip);
				reader.Read((uint)length, out l, out r);
			}

			if(header.IsStereo)
				return new WaveTime(header, l, r);
			return new WaveMonaural(header, l);
		}//Read

		/// <summary>
		/// ファイルから Wave データを読み出し。
		/// (後ろ0詰め)
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="dstLength">コピー後の配列の長さ</param>
		/// <returns>読み出したデータ</returns>
		static WaveData Read(string filename, int skip, int length, int dstLength)
		{
			WaveReader reader = null;
			FormatHeader header;
			double[] l;
			double[] r;

			using(reader = new WaveReader(filename))
			{
				header = reader.Header;
				reader.Skip(skip);
				reader.Read((uint)length, out l, out r);
			}

			if(header.IsStereo)
				return new WaveTime(header, CopyArray(l, 0, length, dstLength), CopyArray(r, 0, length, dstLength));
			return new WaveMonaural(header, CopyArray(l, 0, length, dstLength));
		}//Read

		/// <summary>
		/// ファイルから Wave データを読み出し。
		/// (後ろ0詰め)
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="threshold">閾値。この値以下の音は無音とみなす</param>
		/// <param name="relative">閾値にピーク値からみた相対値を使う</param>
		/// <param name="dstLength">コピー後の配列の長さ</param>
		/// <returns>読み出したデータ</returns>
		static WaveData Read(
			string filename, int skip, int length,
			double threshold ,bool relative, int dstLength)
		{
			WaveReader reader = null;
			FormatHeader header;
			double[] l;
			double[] r;

			using(reader = new WaveReader(filename))
			{
				header = reader.Header;
				reader.Read(reader.Length, out l, out r);
			}

			if(relative) threshold *= Math.Max(GetPeekValue(l), GetPeekValue(r));
			skip += Math.Min(GetSilentLength(l, threshold), GetSilentLength(r, threshold));
			if(skip < 0 || skip + length >= l.Length) skip = 0;

			if(header.IsStereo)
				return new WaveTime(header, CopyArray(l, skip, length, dstLength), CopyArray(r, skip, length, dstLength));
			return new WaveMonaural(header, CopyArray(l, skip, length, dstLength));
		}//Read


		/// <summary>
		/// ファイルから Wave データを読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="threshold">閾値。この値以下の音は無音とみなす</param>
		/// <param name="relative">閾値にピーク値からみた相対値を使う</param>
		/// <returns>読み出したデータ</returns>
		static WaveData Read(
			string filename, int skip, int length,
			double threshold ,bool relative)
		{
			return Read(filename, skip, length, threshold, relative, length);
		}

		/// <summary>
		/// データ wave の読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		public void ReadData(string filename, int skip, int length)
		{
			this.data = WaveAnalyzer.Read(filename, skip, length);
		}

		/// <summary>
		/// データ wave の読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="dstLength">コピー後の配列の長さ</param>
		public void ReadData(string filename, int skip, int length, int dstLength)
		{
			this.data = WaveAnalyzer.Read(filename, skip, length, dstLength);
		}

		/// <summary>
		/// データ wave の読み出し。
		/// 無音区間の除去を行う。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="threshold">閾値</param>
		/// <param name="relative">閾値にピーク値から見た相対値を使う</param>
		public void ReadData(string filename, int skip, int length, double threshold, bool relative)
		{
			this.data = WaveAnalyzer.Read(filename, skip, length, threshold, relative);
		}

		/// <summary>
		/// データ wave の読み出し。
		/// 無音区間の除去を行う。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="threshold">閾値</param>
		/// <param name="relative">閾値にピーク値から見た相対値を使う</param>
		/// <param name="dstLength">コピー後の配列の長さ</param>
		public void ReadData(string filename, int skip, int length, double threshold, bool relative, int dstLength)
		{
			this.data = WaveAnalyzer.Read(filename, skip, length, threshold, relative, dstLength);
		}

		/// <summary>
		/// データ wave の書き込み。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		public void WirteData(string filename)
		{
			using(WaveWriter writer = new WaveWriter(filename, this.data.Header))
			{
				writer.Write(this.data.TimeL, this.data.TimeR);
			}
		}
	}//class WaveAnalyzer
}//namespace WaveAnalysis
