using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// ˆê—l—”—ñ¶¬
	/// </summary>
	public class RandomGenerator : IDataGenerator
	{
		Random rnd;
		int seed;

		public RandomGenerator(){this.rnd = new Random();}
		public RandomGenerator(int seed)
		{
			this.seed = seed;
			this.rnd = new Random(seed);
		}

		public double Next()
		{
			int i = rnd.Next(ushort.MinValue, ushort.MaxValue);
			return i;
		}

		public void Reset()
		{
			this.rnd = new Random(seed);
		}

		public object Clone()
		{
			return new RandomGenerator(this.seed);
		}
	}
}//namespace Data
