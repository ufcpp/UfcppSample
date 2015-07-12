using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// 1音を表す。
	/// </summary>
	public abstract class Sound
	{
		/// <summary>
		/// 音の長さ。
		/// </summary>
		public abstract int Length{get;}

		/// <summary>
		/// 配列化。
		/// </summary>
		/// <returns>音データを格納した配列</returns>
		public abstract double[] ToArray();
	}//class Sound
}//namespace Music
