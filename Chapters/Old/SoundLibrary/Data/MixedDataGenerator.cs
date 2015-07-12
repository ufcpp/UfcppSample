using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// 複数の IDataGenerator の出力値を混ぜ合わせたデータを生成。
	/// </summary>
	public class MixedDataGenerator : IDataGenerator
	{
		IDataGenerator[] generators;

		public MixedDataGenerator(params IDataGenerator[] generators)
		{
			this.generators = generators;
		}

		public double Next()
		{
			double x = 0;
			foreach(IDataGenerator gen in this.generators)
			{
				x += gen.Next();
			}
			return x;
		}

		public void Reset()
		{
			foreach(IDataGenerator gen in this.generators)
				gen.Reset();
		}

		public object Clone()
		{
			IDataGenerator[] gen = new IDataGenerator[this.generators.Length];
			for(int i=0; i<this.generators.Length; ++i)
			{
				gen[i] = (IDataGenerator)this.generators[i].Clone();
			}
			return new MixedDataGenerator(gen);
		}
	}
}//namespace Data
