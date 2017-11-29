using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// ステップ関数生成
	/// </summary>
	public class StepGenerator : IDataGenerator
	{
		bool first = true;
		double gain;
		public StepGenerator() : this(short.MaxValue / 100){}
		public StepGenerator(double gain){this.gain = gain;}

		public double Next()
		{
			if(first)
			{
				first = false;
				return 0;
			}
			return this.gain;
		}

		public void Reset()
		{
			this.first = true;
		}

		public object Clone()
		{
			return new StepGenerator(this.gain);
		}
	}
}//namespace Data
