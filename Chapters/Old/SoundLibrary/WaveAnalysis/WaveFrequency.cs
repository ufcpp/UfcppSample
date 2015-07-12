using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave ƒf[ƒ^Ši”[—pƒNƒ‰ƒXB
	/// ü”g”Œn—ñ‚Åƒf[ƒ^‚ğ•ÛB
	/// </summary>
	public class WaveFrequency : WaveData
	{
		Spectrum l; // L ch ü”g”Œn—ñ
		Spectrum r; // R ch ü”g”Œn—ñ

		public WaveFrequency(){}

		public WaveFrequency(FormatHeader header, Spectrum l, Spectrum r) : base(header)
		{
			this.l = l;
			this.r = r;
		}

		public override double[] TimeL
		{
			set{this.l = Spectrum.FromTimeSequence(value);}
			get{return this.l.TimeSequence;}
		}

		public override double[] TimeR
		{
			set{this.r = Spectrum.FromTimeSequence(value);}
			get{return this.r.TimeSequence;}
		}

		public override int TimeLength
		{
			get
			{
				return this.l.TimeLength;
			}
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
	}
}
