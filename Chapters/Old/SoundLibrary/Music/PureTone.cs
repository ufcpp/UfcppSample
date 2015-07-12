using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// ‹ÉŒ`®‚ÅU•‚ÆˆÊ‘Š‚ğ•Û‚·‚é\‘¢‘ÌB
	/// </summary>
	public struct PolarParameter
	{
		public double amplitude;
		public double phase;

		public PolarParameter(double amp, double phase)
		{
			this.amplitude = amp;
			this.phase     = phase;
		}
	}

	/// <summary>
	/// ³Œ·”g’P‰¹B
	/// </summary>
	public class PureTone : Sound
	{
		int length;       // ‰¹‚Ì’·‚³
		double frequency; // ü”g”(³‹K‰»Špü”g”)
		double amplitude; // U•(ƒŠƒjƒA’l)
		double phase;     // ‰ŠúˆÊ‘Š(rad)

		/// <summary>
		/// U•Aü”g”‚ğw’è‚µ‚Ä‰Šú‰»B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³(ƒTƒ“ƒvƒ‹”)</param>
		/// <param name="freq">ü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amp">U•(ƒŠƒjƒA’l)</param>
		public PureTone(int length, double freq, double amp)
		{
			this.length = length;
			this.frequency = freq;
			this.amplitude = amp;
			this.phase = 0;
		}

		/// <summary>
		/// U•Aü”g”A‰ŠúˆÊ‘Š‚ğw’è‚µ‚Ä‰Šú‰»B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³(ƒTƒ“ƒvƒ‹”)</param>
		/// <param name="freq">ü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amp">U•(ƒŠƒjƒA’l)</param>
		/// <param name="phase">‰ŠúˆÊ‘Š(rad)</param>
		public PureTone(int length, double freq, double amp, double phase)
		{
			this.length = length;
			this.frequency = freq;
			this.amplitude = amp;
			this.phase = phase;
		}

		/// <summary>
		/// U•Aü”g”A‰ŠúˆÊ‘Š‚ğw’è‚µ‚Ä‰Šú‰»B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³(ƒTƒ“ƒvƒ‹”)</param>
		/// <param name="freq">ü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="parameter">U•‚Æ‰ŠúˆÊ‘Š</param>
		public PureTone(int length, double freq, PolarParameter parameter)
			: this(length, freq, parameter.amplitude, parameter.phase)
		{
		}

		public override int Length
		{
			get{return this.length;}
		}

		public double this[int i]
		{
			get
			{
				return this.amplitude * Math.Sin(this.frequency * i + this.phase);
			}
		}

		public override double[] ToArray()
		{
			double[] array = new double[this.Length];
			for(int i=0; i<this.Length; ++i)
				array[i] = this[i];
			return array;
		}

	}//class PureTone
}
