using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// 極形式で振幅と位相を保持する構造体。
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
	/// 正弦波単音。
	/// </summary>
	public class PureTone : Sound
	{
		int length;       // 音の長さ
		double frequency; // 周波数(正規化角周波数)
		double amplitude; // 振幅(リニア値)
		double phase;     // 初期位相(rad)

		/// <summary>
		/// 振幅、周波数を指定して初期化。
		/// </summary>
		/// <param name="length">音の長さ(サンプル数)</param>
		/// <param name="freq">周波数(正規化角周波数)</param>
		/// <param name="amp">振幅(リニア値)</param>
		public PureTone(int length, double freq, double amp)
		{
			this.length = length;
			this.frequency = freq;
			this.amplitude = amp;
			this.phase = 0;
		}

		/// <summary>
		/// 振幅、周波数、初期位相を指定して初期化。
		/// </summary>
		/// <param name="length">音の長さ(サンプル数)</param>
		/// <param name="freq">周波数(正規化角周波数)</param>
		/// <param name="amp">振幅(リニア値)</param>
		/// <param name="phase">初期位相(rad)</param>
		public PureTone(int length, double freq, double amp, double phase)
		{
			this.length = length;
			this.frequency = freq;
			this.amplitude = amp;
			this.phase = phase;
		}

		/// <summary>
		/// 振幅、周波数、初期位相を指定して初期化。
		/// </summary>
		/// <param name="length">音の長さ(サンプル数)</param>
		/// <param name="freq">周波数(正規化角周波数)</param>
		/// <param name="parameter">振幅と初期位相</param>
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
