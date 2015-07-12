using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// ミキサー。
	/// 並列接続＋ゲイン調整。
	/// </summary>
	public class Mixer : IFilter
	{
		public struct Tuple
		{
			public IFilter filter;
			public double gain;

			public Tuple(IFilter filter, double gain)
			{
				this.filter = filter;
				this.gain = gain;
			}
		}
		Tuple[] filters;

		public Mixer(params Tuple[] filters)
		{
			this.filters = filters;

			this.Clear();
		}

		public double GetValue(double x)
		{
			double tmp = 0;
			foreach(Tuple t in this.filters)
				tmp += t.filter.GetValue(x) * t.gain;
			return tmp;
		}

		public void Clear()
		{
			foreach(Tuple t in this.filters)
				t.filter.Clear();
		}

		public object Clone()
		{
			Tuple[] clone = new Tuple[this.filters.Length];
			for(int i=0; i<clone.Length; ++i)
			{
				clone[i] = new Tuple(
					(IFilter)this.filters[i].filter.Clone(),
					this.filters[i].gain);
			}

			return new Mixer(clone);
		}
	}//class Mixer

	/// <summary>
	/// 遅延付きミキサー。
	/// 並列接続＋ゲイン＆遅延調整。
	/// </summary>
	public class DelayMixer : IFilter
	{
		public struct Tuple
		{
			public IFilter filter;
			public double gain;
			public int delay;

			public Tuple(IFilter filter, double gain, int delay)
			{
				this.filter = filter;
				this.gain = gain;
				this.delay = delay;
			}
		}
		Tuple[] filters;
		CircularBuffer buf;

		public DelayMixer(params Tuple[] filters)
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

		public double GetValue(double x)
		{
			this.buf.PushBack(0);

			foreach(Tuple t in this.filters)
				this.buf[t.delay] += t.filter.GetValue(x) * t.gain;
			return this.buf[0];
		}

		public void Clear()
		{
			foreach(Tuple t in this.filters)
			{
				this.buf.PushFront(0);
				t.filter.Clear();
			}
		}

		public object Clone()
		{
			Tuple[] clone = new Tuple[this.filters.Length];
			for(int i=0; i<clone.Length; ++i)
			{
				clone[i] = new Tuple(
					(IFilter)this.filters[i].filter.Clone(),
					this.filters[i].gain,
					this.filters[i].delay);
			}

			return new DelayMixer(clone);
		}
	}//class DelayMixer
}
