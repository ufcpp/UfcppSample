using System;

using SoundLibrary.Filter;

namespace SoundLibrary.Music
{
	/// <summary>
	/// フィルタを掛けた音を生成する。
	/// </summary>
	public class SoundWithFilter : Sound
	{
		Sound sound;
		IFilter filter;
		int delay; // filter の遅延時間

		/// <summary>
		/// 元となる Sound、フィルタ、フィルタの遅延時間を指定して生成。
		/// </summary>
		/// <param name="sound">元となる音</param>
		/// <param name="filter">フィルタ</param>
		/// <param name="delay">filter の遅延時間</param>
		public SoundWithFilter(Sound sound, IFilter filter, int delay)
		{
			this.sound = sound;
			this.filter = filter;
			this.delay = delay;
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

			int i=0;
			int j=0;
			for(; i<this.delay; ++i)
			{
				filter.GetValue(x[i]);
			}
			for(; i<x.Length; ++i, ++j)
			{
				x[j] = filter.GetValue(x[i]);
			}
			for(; j<x.Length; ++j)
			{
				x[j] = filter.GetValue(0);
			}

			return x;
		}
	}
}
