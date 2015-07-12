using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// 時系列でデータを保持。
	/// </summary>
	public class WaveTime : WaveData
	{
		double[] l;  // L ch 時系列
		double[] r;  // R ch 時系列

		public WaveTime(){}

		public WaveTime(FormatHeader header, double[] l, double[] r) : base(header)
		{
			this.l = l;
			this.r = r;
		}

		public override double[] TimeL
		{
			set{this.l = value;}
			get{return this.l;}
		}

		public override double[] TimeR
		{
			set{this.r = value;}
			get{return this.r;}
		}

		public override Spectrum Left
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l);}
		}

		public override Spectrum Right
		{
			set{this.r = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.r);}
		}
	}
}
