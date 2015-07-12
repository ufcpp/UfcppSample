using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// ³Œ·”g”{‰¹—ñB
	/// </summary>
	public class HarmonicTone : Sound
	{
		int length;       // ‰¹‚Ì’·‚³
		double frequency; // Šî’ê‰¹‚Ìü”g”(ƒÎ‚Å³‹K‰»)
		PolarParameter[] parameters;

		/// <summary>
		/// ‰¹‚Ì’·‚³AŠî’ê‰¹‚Ìü”g”A”{‰¹—ñ‚ÌU•EˆÊ‘Š‚©‚ç¶¬B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³</param>
		/// <param name="freq">Šî’ê‰¹‚Ìü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="parameters">”{‰¹—ñ‚ÌU•(ƒŠƒjƒA’l)‚ÆˆÊ‘Š(rad)</param>
		public HarmonicTone(int length, double freq, params PolarParameter[] parameters)
		{
			this.length = length;
			this.frequency = freq;
			this.parameters = parameters;
		}

		/// <summary>
		/// ‰¹‚Ì’·‚³AŠî’ê‰¹‚Ìü”g”A”{‰¹—ñ‚ÌU•‚©‚ç¶¬B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³</param>
		/// <param name="freq">Šî’ê‰¹‚Ìü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amps">”{‰¹—ñ‚ÌU•(ƒŠƒjƒA’l)</param>
		public HarmonicTone(int length, double freq, params double[] amps)
		{
			this.length = length;
			this.frequency = freq;
			this.parameters = new PolarParameter[amps.Length];
			for(int i=0; i<amps.Length; ++i)
			{
				this.parameters[i] = new PolarParameter(amps[i], 0);
			}
		}

		/// <summary>
		/// ‰¹‚Ì’·‚³AŠî’ê‰¹‚Ìü”g”A”{‰¹—ñ‚ÌU•EˆÊ‘Š‚©‚ç¶¬B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³</param>
		/// <param name="freq">Šî’ê‰¹‚Ìü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amps">”{‰¹—ñ‚ÌU•(ƒŠƒjƒA’l)</param>
		/// <param name="phase">”{‰¹—ñ‚ÌˆÊ‘Š</param>
		public HarmonicTone(int length, double freq, double[] amps, double[] phase)
		{
			this.length = length;
			this.frequency = freq;
			this.parameters = new PolarParameter[amps.Length];
			for(int i=0; i<amps.Length; ++i)
			{
				this.parameters[i] = new PolarParameter(amps[i], phase[i]);
			}
		}

		public override int Length
		{
			get{return this.length;}
		}

		public double this[int i]
		{
			get
			{
				double x = 0;
				for(int k=0; k<this.parameters.Length; ++k)
				{
					PolarParameter parameter = this.parameters[k];
					x += parameter.amplitude * Math.Sin(this.frequency * k * i + parameter.phase);
				}

				return x;
			}
		}

		public override double[] ToArray()
		{
			double[] array = new double[this.Length];
			for(int i=0; i<this.Length; ++i)
				array[i] = this[i];
			return array;
		}
	}
}
