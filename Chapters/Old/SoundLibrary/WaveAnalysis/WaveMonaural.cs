using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// モノラル版。
	/// </summary>
	public class WaveMonaural : WaveData
	{
		double[] l;  // L ch 時系列

		public WaveMonaural(){}

		public WaveMonaural(FormatHeader header, double[] l) : base(header)
		{
			this.l = l;
		}

		public override double[] TimeL
		{
			set{this.l = value;}
			get{return this.l;}
		}

		public override double[] TimeR
		{
			set{this.l = value;}
			get{return this.l;}
		}

		public override Spectrum Left
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l);}
		}

		public override Spectrum Right
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l);}
		}

		public override void SetLR(Spectrum left, Spectrum right)
		{
			this.l = (0.5 * (left + right)).TimeSequence;
		}

		public override void SetMS(Spectrum left, Spectrum right)
		{
			this.l = (0.5 * (left + right)).TimeSequence;
		}

		public override Spectrum Middle
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l);}
		}

		public override Spectrum Side
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l);}
		}

		public override void Mul(Spectrum s)
		{
			this.Left *= s;
		}

		public override void Div(Spectrum s)
		{
			this.Left /= s;
		}
	}
}
