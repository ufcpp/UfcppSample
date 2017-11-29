using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// うなり生成用パラメータ。
	/// </summary>
	public class TremoloParameter
	{
		public double rate;  // うなりの周波数(正規化角周波数)。
		public double depth; // うなりの深さ(0～1)。
		public int    delay; // うなりがかかり始める時間(ステップ数)。

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="rate">うなりの周波数(正規化角周波数)</param>
		/// <param name="depth">うなりの深さ(0～1)</param>
		/// <param name="delay">うなりがかかり始める時間(ステップ数)</param>
		public TremoloParameter(double rate, double depth, int delay)
		{
			this.rate = rate;
			this.depth = depth;
			this.delay = delay;
		}
	}

	/// <summary>
	/// 元となる Sound にうなりを掛けた Sound を生成する。
	/// </summary>
	public class Tremolo : Sound
	{
		TremoloParameter parameter;
		Sound sound;

		/// <summary>
		/// うなりのパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="parameter">うなりのパラメータ</param>
		/// <param name="sound">元となる音</param>
		public Tremolo(TremoloParameter parameter, Sound sound)
		{
			if(sound.Length < parameter.delay)
				throw new ArgumentException("音が短すぎ");

			this.parameter = parameter;
			this.sound = sound;
		}

		/// <summary>
		/// うなりパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="rate">うなりの周波数(正規化角周波数)</param>
		/// <param name="depth">うなりの深さ(0～1)</param>
		/// <param name="delay">うなりがかかり始める時間(ステップ数)</param>
		/// <param name="sound">元となる音</param>
		public Tremolo(double rate, double depth, int delay, Sound sound)
			: this(new TremoloParameter(rate, depth, delay), sound)
		{
		}


		public override int Length
		{
			get
			{
				return this.sound.Length;
			}
		}

		public override double[] ToArray()
		{
			double[] x = this.sound.ToArray();

			int delay = this.parameter.delay;
			double rate = this.parameter.rate;
			double depth = this.parameter.depth;

			for(int i=delay+1; i<x.Length; ++i)
			{
				x[i] *= 1 + depth * Math.Sin(rate * (i - delay));
			}

			return x;
		}

	}
}
