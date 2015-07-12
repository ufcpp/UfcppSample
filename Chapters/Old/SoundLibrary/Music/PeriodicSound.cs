using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// üŠú‰¹‚ğì‚éB
	/// </summary>
	public class PeriodicSound : Sound
	{
		int iteration;
		Sound sound;

		/// <summary>
		/// sound ‚ğ iteration ‰ñŒJ‚è•Ô‚·‰¹‚ğì‚éB
		/// </summary>
		/// <param name="iteration">”½•œ‰ñ”</param>
		/// <param name="sound">Œ´‰¹</param>
		public PeriodicSound(int iteration, Sound sound)
		{
			this.iteration = iteration;
			this.sound = sound;
		}

		public override int Length
		{
			get
			{
				return this.sound.Length * this.iteration;
			}
		}

		public override double[] ToArray()
		{
			double[] w = this.sound.ToArray();
			double[] x = new double[this.Length];

			for(int i=0, j=0; i<iteration; ++i)
				for(int k=0; k<w.Length; ++k, ++j)
					x[j] = w[k];
			return x;
		}
	}
}
