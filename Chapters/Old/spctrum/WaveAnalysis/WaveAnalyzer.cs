using System;

using SpectrumAnalysis;
using Wave;

namespace WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// 周波数系列でデータを保持。
	/// </summary>
	class WaveFrequency : WaveData
	{
		Spectrum l; // L ch 周波数系列
		Spectrum r; // R ch 周波数系列

		public WaveFrequency(){}

		public WaveFrequency(FormatHeader header, Spectrum l, Spectrum r) : base(header)
		{
			this.l = l;
			this.r = r;
		}

		public override double[] TimeL
		{
			set{this.l = Spectrum.FromTimeSequence(value, this.Header.sampleRate);}
			get{return this.l.TimeSequence;}
		}

		public override double[] TimeR
		{
			set{this.r = Spectrum.FromTimeSequence(value, this.Header.sampleRate);}
			get{return this.r.TimeSequence;}
		}

		public override Spectrum Left
		{
			set{this.l = value;}
			get{return this.l;}
		}

		public override Spectrum Right
		{
			set{this.r = value;}
			get{return this.r;}
		}
	}//class WaveFrequency

	/// <summary>
	/// リファレンス音のタイプ
	/// </summary>
	public enum ReferenceType
	{
		Both,    // 両 ch 使う
		Left,    // L ch だけ使う
		Right,   // R ch だけ使う
		Reverse, // L,R 逆側 ch を使う
		Cross,   // L が直接音、R がクロストーク音のとき
		// (L がクロストーク音、R が直接音でも結果は同じ)
		// a = data.L, b = data.R
		// c = ref.L,  d = ref.R
		// [a b][c d]-1              [ac-bd bc-ad]
		// [b a][d c]   = 1/(c^2-d^2)[bc-ad ac-bd]
		// ∴
		// data.L = (ac-bd) / (c^2-d^2)
		// data.R = (bc-ad) / (c^2-d^2)
	}//enum ReferenceType

	/// <summary>
	/// Wave ファイル解析用クラス。
	/// </summary>
	public class WaveAnalyzer
	{
		WaveData data = new WaveTime(); // データ音

		/// <summary>
		/// データ Wave の取得
		/// </summary>
		public WaveData Data{get{return this.data;}}

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

			return new WaveTime(header, l, r);
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
			WaveReader reader = null;
			FormatHeader header;
			double[] l;
			double[] r;

			using(reader = new WaveReader(filename))
			{
				header = reader.Header;
				reader.Skip(0);
				reader.Read(reader.Length, out l, out r);
			}

			if(relative) threshold *= Math.Max(GetPeekValue(l), GetPeekValue(r));
			skip += Math.Min(GetSilentLength(l, threshold), GetSilentLength(r, threshold));
			if(skip < 0 || skip + length >= l.Length) skip = 0;

			return new WaveTime(header, CopyArray(l, skip, length), CopyArray(r, skip, length));
		}//Read

		/// <summary>
		/// ファイルから Wave データを読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="threshold">閾値。この値以下の音は無音とみなす</param>
		/// <param name="relative">閾値にピーク値からみた相対値を使う</param>
		/// <param name="type">リファレンスのタイプ</param>
		/// <returns>読み出したデータ</returns>
		static WaveData Read(
			string filename, int skip, int length,
			double threshold ,bool relative, ReferenceType type)
		{
			WaveReader reader = null;
			FormatHeader header;
			double[] l;
			double[] r;

			using(reader = new WaveReader(filename))
			{
				header = reader.Header;

				// R ch を使いたいのに wave ファイルがモノラルの場合
				if(type != ReferenceType.Left && reader.Header.ch == 1)
				{
					return new WaveTime();
				}

				reader.Skip(0);
				reader.Read(reader.Length, out l, out r);
			}

			if(type == ReferenceType.Left)
			{
				if(relative) threshold *= GetPeekValue(l);
				skip += GetSilentLength(l, threshold);
			}
			else if(type == ReferenceType.Right)
			{
				if(relative) threshold *= GetPeekValue(r);
				skip += GetSilentLength(r, threshold);
			}
			else
			{
				if(relative) threshold *= Math.Max(GetPeekValue(l), GetPeekValue(r));
				skip += Math.Min(GetSilentLength(l, threshold), GetSilentLength(r, threshold));
			}
			if(skip < 0 || skip + length >= l.Length) skip = 0;

			return new WaveTime(header, CopyArray(l, skip, length), CopyArray(r, skip, length));
		}//Read

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

		static void Deconvolute(ref WaveData data, WaveData reference, ReferenceType type, bool isNormalize)
		{
			data = data.ToSpectrum();
			reference =reference.ToSpectrum();

			if(isNormalize)
			{
				reference.Left.Normalize();
				reference.Right.Normalize();
			}

			switch(type)
			{
				case ReferenceType.Left:
					data.Left /= reference.Left;
					data.Right /= reference.Left;
					break;
				case ReferenceType.Right:
					data.Left /= reference.Right;
					data.Right /= reference.Right;
					break;
				case ReferenceType.Both:
					data.Left /= reference.Left;
					data.Right /= reference.Right;
					break;
				case ReferenceType.Cross:
					Spectrum a = data.Left;
					Spectrum b = data.Right;
					Spectrum c = reference.Left;
					Spectrum d = reference.Right;
					Spectrum det = c*c - d*d;
					data.Left  = (a*c - b*d) / det;
					data.Right = (b*c - a*d) / det;
					break;
			}
		}

		/// <summary>
		/// リファレンス wave の読み出し。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="type">リファレンスのタイプ</param>
		/// <param name="isNormalize">リファレンスの振幅特性を正規化するかどうか</param>
		public void DeconvoluteReference(
			string filename, int skip, int length,
			ReferenceType type, bool isNormalize)
		{
			WaveData reference = WaveAnalyzer.Read(filename, skip, length);
			Deconvolute(ref this.data, reference, type, isNormalize);
		}

		/// <summary>
		/// リファレンス wave の読み出し。
		/// 無音区間の除去を行う。
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="length">読み込む長さ</param>
		/// <param name="skip">ファイルの先頭を読み飛ばす長さ</param>
		/// <param name="type">リファレンスのタイプ</param>
		/// <param name="isNormalize">リファレンスの振幅特性を正規化するかどうか</param>
		/// <param name="threshold">閾値</param>
		/// <param name="relative">閾値にピーク値から見た相対値を使う</param>
		public void DeconvoluteReference(
			string filename, int skip, int length,
			ReferenceType type, bool isNormalize,
			double threshold, bool relative)
		{
			WaveData reference = WaveAnalyzer.Read(filename, skip, length, threshold, relative, type);
			Deconvolute(ref this.data, reference, type, isNormalize);
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
