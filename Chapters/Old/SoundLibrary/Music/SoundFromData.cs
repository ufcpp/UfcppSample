using System;

using SoundLibrary.Data;

namespace SoundLibrary.Music
{
	/// <summary>
	/// IDataGenerator から Sound を作る。
	/// </summary>
	public class SoundFromData : Sound
	{
		IDataGenerator generator;
		int length;

		/// <summary>
		/// IDataGenerator と音の長さを指定して初期化。
		/// </summary>
		/// <param name="generator">データ生成クラス</param>
		/// <param name="length">音の長さ</param>
		public SoundFromData(IDataGenerator generator, int length)
		{
			this.generator = generator;
			this.length = length;
		}

		public override int Length
		{
			get
			{
				return this.length;
			}
		}

		public override double[] ToArray()
		{
			double[] x = new double[this.length];

			for(int i=0; i<this.length; ++i)
				x[i] = this.generator.Next();

			return x;
		}
	}
}
