using System;

namespace SoundLibrary.Data
{
	/// <summary>
	/// インパルス列生成
	/// </summary>
	public class ImpulseGenerator : IDataGenerator
	{
		double gain;
		bool first = true;
		public ImpulseGenerator () : this(short.MaxValue){}
		public ImpulseGenerator (double gain){this.gain = gain;}
		public double Next()
		{
			if(first)
			{
				first = false;
				return this.gain;
			}
			return 0;
		}

		public void Reset()
		{
			this.first = true;
		}

		public object Clone()
		{
			return new ImpulseGenerator(this.gain);
		}
	}
}//namespace Data
