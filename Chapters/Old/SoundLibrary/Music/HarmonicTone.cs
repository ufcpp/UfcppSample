using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// 正弦波倍音列。
	/// </summary>
	public class HarmonicTone : Sound
	{
		int length;       // 音の長さ
		double frequency; // 基底音の周波数(πで正規化)
		PolarParameter[] parameters;

		/// <summary>
		/// 音の長さ、基底音の周波数、倍音列の振幅・位相から生成。
		/// </summary>
		/// <param name="length">音の長さ</param>
		/// <param name="freq">基底音の周波数(正規化角周波数)</param>
		/// <param name="parameters">倍音列の振幅(リニア値)と位相(rad)</param>
		public HarmonicTone(int length, double freq, params PolarParameter[] parameters)
		{
			this.length = length;
			this.frequency = freq;
			this.parameters = parameters;
		}

		/// <summary>
		/// 音の長さ、基底音の周波数、倍音列の振幅から生成。
		/// </summary>
		/// <param name="length">音の長さ</param>
		/// <param name="freq">基底音の周波数(正規化角周波数)</param>
		/// <param name="amps">倍音列の振幅(リニア値)</param>
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
		/// 音の長さ、基底音の周波数、倍音列の振幅・位相から生成。
		/// </summary>
		/// <param name="length">音の長さ</param>
		/// <param name="freq">基底音の周波数(正規化角周波数)</param>
		/// <param name="amps">倍音列の振幅(リニア値)</param>
		/// <param name="phase">倍音列の位相</param>
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
