using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// ‡¬‰¹B
	/// •¡”‚Ì Sound ‚ğ‡¬B
	/// </summary>
	public class CompoundSound : Sound
	{
		Sound[] sounds;

		/// <summary>
		/// •¡”‚Ì Sound ‚©‚ç¶¬B
		/// </summary>
		/// <param name="sounds">‡¬‰¹‚ğ\¬‚·‚é‰¹B</param>
		/// <exception cref="ArgumentException">sounds ‚Ì’·‚³‚ª‚»‚ë‚Á‚Ä‚¢‚È‚¢‚Æ‚«‚É”­¶‚µ‚Ü‚·B</exception>
		public CompoundSound(params Sound[] sounds)
		{
			int len = sounds[0].Length;
			foreach(Sound sound in sounds)
			{
				if(len != sound.Length)
					throw new ArgumentException("sounds ‚Ì’·‚³‚ª‚»‚ë‚Á‚Ä‚¢‚Ü‚¹‚ñB");
			}

			this.sounds = sounds;
		}

		/// <summary>
		/// ’·˜a‰¹¶¬B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³</param>
		/// <param name="freq">ˆê”Ô‰º‚Ì‰¹‚Ìü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amp">U•(ƒŠƒjƒA’l)</param>
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
		/// ’P˜a‰¹¶¬B
		/// </summary>
		/// <param name="length">‰¹‚Ì’·‚³</param>
		/// <param name="freq">ˆê”Ô‰º‚Ì‰¹‚Ìü”g”(³‹K‰»Špü”g”)</param>
		/// <param name="amp">U•(ƒŠƒjƒA’l)</param>
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
