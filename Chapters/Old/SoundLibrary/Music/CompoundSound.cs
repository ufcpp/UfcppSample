using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// 合成音。
	/// 複数の Sound を合成。
	/// </summary>
	public class CompoundSound : Sound
	{
		Sound[] sounds;

		/// <summary>
		/// 複数の Sound から生成。
		/// </summary>
		/// <param name="sounds">合成音を構成する音。</param>
		/// <exception cref="ArgumentException">sounds の長さがそろっていないときに発生します。</exception>
		public CompoundSound(params Sound[] sounds)
		{
			int len = sounds[0].Length;
			foreach(Sound sound in sounds)
			{
				if(len != sound.Length)
					throw new ArgumentException("sounds の長さがそろっていません。");
			}

			this.sounds = sounds;
		}

		/// <summary>
		/// 長和音生成。
		/// </summary>
		/// <param name="length">音の長さ</param>
		/// <param name="freq">一番下の音の周波数(正規化角周波数)</param>
		/// <param name="amp">振幅(リニア値)</param>
		/// <returns></returns>
		public static Sound MajorChord(int length, double freq, double amp)
		{
			return new CompoundSound(
				new PureTone(length, freq, amp / 3),
				new PureTone(length, freq * PureTemperament.MAJOR3, amp / 3),
				new PureTone(length, freq * PureTemperament.MINOR3, amp / 3)
				);
		}

		/// <summary>
		/// 単和音生成。
		/// </summary>
		/// <param name="length">音の長さ</param>
		/// <param name="freq">一番下の音の周波数(正規化角周波数)</param>
		/// <param name="amp">振幅(リニア値)</param>
		/// <returns></returns>
		public static Sound MinorChord(int length, double freq, double amp)
		{
			return new CompoundSound(
				new PureTone(length, freq, amp / 3),
				new PureTone(length, freq * PureTemperament.MINOR3, amp / 3),
				new PureTone(length, freq * PureTemperament.MAJOR3, amp / 3)
				);
		}

		public override int Length
		{
			get
			{
				return this.sounds[0].Length;
			}
		}

		public override double[] ToArray()
		{
			double[] x = new double[this.Length];
			foreach(Sound sound in this.sounds)
			{
				double[] y = sound.ToArray();
				for(int i=0; i<x.Length; ++i)
					x[i] += y[i];
			}
			return x;
		}

	}//class CompoundSound
}
