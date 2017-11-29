using System;
using SoundLibrary.Filter.Delay;

namespace SoundLibrary.Music
{
	/// <summary>
	/// ビブラート生成用パラメータ。
	/// </summary>
	public class VibratoParameter
	{
		public double rate;  // ビブラートレート(正規化角周波数)。
		public int    depth; // ビブラートデプス(ステップ数)。
		public int    delay; // ビブラートディレイ(ステップ数)。

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="rate">ビブラートレート(正規化角周波数)</param>
		/// <param name="depth">ビブラートデプス(ステップ数)</param>
		/// <param name="delay">ビブラートディレイ(ステップ数)</param>
		public VibratoParameter(double rate, int depth, int delay)
		{
			this.rate = rate;
			this.depth = depth;
			this.delay = delay;
		}
	}

	/// <summary>
	/// 元となる Sound にビブラート曲線を掛けた Sound を生成する。
	/// </summary>
	public class Vibrato : Sound
	{
		VibratoParameter parameter;
		Sound sound;
		FractionalDelay delay;

		/// <summary>
		/// ビブラートパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="parameter">ビブラートパラメータ</param>
		/// <param name="sound">元となる音</param>
		public Vibrato(VibratoParameter parameter, Sound sound)
		{
			if(sound.Length < parameter.delay)
				throw new ArgumentException("音が短すぎ");

			this.parameter = parameter;
			this.sound = sound;

			this.delay = new FractionalDelay(2 * parameter.depth);
			this.delay.DelayTime = parameter.depth;
		}

		/// <summary>
		/// ビブラートパラメータと元となる Sound を指定して生成。
		/// </summary>
		/// <param name="rate">ビブラートレート(正規化角周波数)</param>
		/// <param name="depth">ビブラートデプス(0～1)</param>
		/// <param name="delay">ビブラートディレイ(ステップ数)</param>
		/// <param name="sound">元となる音</param>
		public Vibrato(double rate, int depth, int delay, Sound sound)
			: this(new VibratoParameter(rate, depth, delay), sound)
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
			int depth = this.parameter.depth;

			int i=0, j=delay;
			for(; i<delay; ++i) this.delay.Push(x[i]);
			for(; i<delay + depth; ++i) this.delay.Push(x[i]);
			for(; i<x.Length; ++j, ++i)
			{
				this.delay.DelayTime = depth * (1 + Math.Sin(rate * (i - delay)));
				x[j] = this.delay.GetValue(x[i]);
			}
			for(; j<x.Length; ++j) x[j] = this.delay.GetValue(0);

			return x;
		}

	}
}
