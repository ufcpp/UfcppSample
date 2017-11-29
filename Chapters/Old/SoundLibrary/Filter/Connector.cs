using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// 直列接続。
	/// </summary>
	public class SerialConnector : IFilter
	{
		IFilter[] filters;

		public SerialConnector(params IFilter[] filters)
		{
			this.filters = filters;

			this.Clear();
		}

		public double GetValue(double x)
		{
			foreach(IFilter f in this.filters)
				x = f.GetValue(x);
			return x;
		}

		public void Clear()
		{
			foreach(IFilter f in this.filters)
				f.Clear();
		}

		public object Clone()
		{
			IFilter[] clone = new IFilter[this.filters.Length];
			for(int i=0; i<clone.Length; ++i)
			{
				clone[i] = (IFilter)this.filters[i].Clone();
			}

			return new SerialConnector(clone);
		}
	}//class SerialConnector

	/// <summary>
	/// 並列接続。
	/// </summary>
	public class PallarelConnector : IFilter
	{
		IFilter[] filters;

		public PallarelConnector(params IFilter[] filters)
		{
			this.filters = filters;

			this.Clear();
		}

		public double GetValue(double x)
		{
			double tmp = 0;
			foreach(IFilter f in this.filters)
				tmp += f.GetValue(x);
			return tmp;
		}

		public void Clear()
		{
			foreach(IFilter f in this.filters)
				f.Clear();
		}

		public object Clone()
		{
			IFilter[] clone = new IFilter[this.filters.Length];
			for(int i=0; i<clone.Length; ++i)
			{
				clone[i] = (IFilter)this.filters[i].Clone();
			}

			return new PallarelConnector(clone);
		}
	}//class PallarelConnector
}//namespace Filter
