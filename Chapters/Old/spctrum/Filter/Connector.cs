using System;

namespace Filter
{
	/// <summary>
	/// Connector の概要の説明です。
	/// </summary>
	public class SerialConnector : IFilter
	{
		IFilter[] filters;

		public SerialConnector(IFilter[] filters)
		{
			this.filters = filters;
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
	}//class SerialConnector

	/// <summary>
	/// Connector の概要の説明です。
	/// </summary>
	public class PallarelConnector : IFilter
	{
		IFilter[] filters;

		public PallarelConnector(IFilter[] filters)
		{
			this.filters = filters;
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
	}//class PallarelConnector

	/// <summary>
	/// Connector の概要の説明です。
	/// </summary>
	public class Mixer : IFilter
	{
		IFilter[] filters;
		double[] gains;

		public Mixer(IFilter[] filters, double[] gains)
		{
			this.filters = filters;
			this.gains = gains;
		}

		public double GetValue(double x)
		{
			double tmp = 0;
			for(int i=0; i<this.filters.Length; ++i)
				tmp += this.filters[i].GetValue(x) * this.gains[i];
			return tmp;
		}

		public void Clear()
		{
			foreach(IFilter f in this.filters)
				f.Clear();
		}
	}//class Mixer
}//namespace Filter
