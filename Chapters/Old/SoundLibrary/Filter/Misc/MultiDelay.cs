using System;

namespace SoundLibrary.Filter.Misc
{
	/// <summary>
	/// マルチディレイ。
	/// </summary>
	public class MultiDelay : IFilter
	{
		public struct Tuple
		{
			public double gain;
			public int delay;

			public Tuple(double gain, int delay)
			{
				this.gain = gain;
				this.delay = delay;
			}
		}
		Tuple[] filters;
		CircularBuffer buf;

		public MultiDelay(params Tuple[] filters)
		{
			this.filters = filters;

			int maxDelay = int.MinValue;
			foreach(Tuple t in filters)
				if(t.delay > maxDelay) maxDelay = t.delay;
			++maxDelay;

			if(maxDelay > 0)
				this.buf = new CircularBuffer(maxDelay);

			this.Clear();
		}

		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			this.buf.PushBack(0);

			foreach(Tuple t in this.filters)
				this.buf[t.delay] += x * t.gain;
			return this.buf[0];
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			for(int i=0; i<this.buf.Length; ++i)
				this.buf[i] = 0;
		}

		public object Clone()
		{
			return new MultiDelay((Tuple[])this.filters.Clone());
		}
	}
}
