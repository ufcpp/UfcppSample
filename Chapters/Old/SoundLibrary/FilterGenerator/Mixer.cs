using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// MixerGenerator を作成する。
	/// </summary>
	public class MixerGenerator : FilterGenerator
	{
		const string FilterName = "ミキサー";
		const string SubFilterName = "フィルタ";
		const string GainName   = "増幅率";

		public MixerGenerator()
		{
			this.arrayProperties = new FilterArrayProperty[1];
			this.arrayProperties[0] = new FilterArrayProperty(
				new FilterArrayProperty.Tuple(SubFilterName, typeof(FilterGenerator)),
				new FilterArrayProperty.Tuple(GainName, typeof(double))
				);
		}

		public override string CheckConstraint()
		{
			for(int i=0; i<this.Length; ++i)
			{
				string message = this.GetSubfilter(i).CheckConstraint();
				if(message != null)
					return message;
			}
			return null;
		}

		public override IFilter GetFilter()
		{
			int len = this.Length;
			Mixer.Tuple[] tuples = new Mixer.Tuple[len];
			for(int i=0; i<len; ++i)
			{
				double gain = this.GetGain(i);
				FilterGenerator gen = this.GetSubfilter(i);
				IFilter filter = gen.GetFilter();
				tuples[i] = new Mixer.Tuple(filter, gain);
			}
			return new Mixer(tuples);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("Mixer");

			int len = this.Length;
			for(int i=0; i<len; ++i)
			{
				xwriter.WriteStartElement("Filter");
				xwriter.WriteAttributeString("gain", this.GetGain(i).ToString());
				FilterGenerator gen = this.GetSubfilter(i);
				gen.ToXml(xwriter);
				xwriter.WriteEndElement();
			}

			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			foreach(XmlNode node in elem.ChildNodes)
			{
				if(node is XmlElement)
				{
					double gain = this.converter.ToPower(node.Attributes["gain"].Value);
					FilterGenerator gen = FilterGenerator.CreateFromXml((XmlElement)node.FirstChild, this.converter);
					this.Add(gen, gain);
				}
			}
		}

		public FilterGenerator GetSubfilter(int i)
		{
			return (FilterGenerator)this.arrayProperties[0][i, 0];
		}

		public double GetGain(int i)
		{
			return (double)this.arrayProperties[0][i, 1];
		}

		public void SetSubfilter(int i, FilterGenerator gen)
		{
			this.arrayProperties[0][i, 0] = gen;
		}

		public int Length
		{
			get{return this.arrayProperties[0].ListLength;}
		}

		public void Add(FilterGenerator gen, double gain)
		{
			this.arrayProperties[0].Add(gen, gain);
		}

		public void Remove(int i)
		{
			this.arrayProperties[0].Remove(i);
		}
	}//class MixerGenerator

	/// <summary>
	/// DelayMixerGenerator を作成する。
	/// </summary>
	public class DelayMixerGenerator : FilterGenerator
	{
		const string FilterName = "遅延付きミキサー";
		const string SubFilterName = "フィルタ";
		const string GainName   = "増幅率";
		const string DelayName  = "遅延サンプル数";

		public DelayMixerGenerator()
		{
			this.arrayProperties = new FilterArrayProperty[1];
			this.arrayProperties[0] = new FilterArrayProperty(
				new FilterArrayProperty.Tuple(SubFilterName, typeof(FilterGenerator)),
				new FilterArrayProperty.Tuple(GainName, typeof(double)),
				new FilterArrayProperty.Tuple(DelayName, typeof(int))
				);
		}

		public override string CheckConstraint()
		{
			for(int i=0; i<this.Length; ++i)
			{
				string message = this.GetSubfilter(i).CheckConstraint();
				if(message != null)
					return message;
			}
			return null;
		}

		public override IFilter GetFilter()
		{
			int len = this.Length;
			DelayMixer.Tuple[] tuples = new DelayMixer.Tuple[len];
			for(int i=0; i<len; ++i)
			{
				double gain = this.GetGain(i);
				int delay = this.GetDelay(i);
				FilterGenerator gen = this.GetSubfilter(i);
				IFilter filter = gen.GetFilter();
				tuples[i] = new DelayMixer.Tuple(filter, gain, delay);
			}
			return new DelayMixer(tuples);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("DelayMixer");

			int len = this.Length;
			for(int i=0; i<len; ++i)
			{
				xwriter.WriteStartElement("Filter");
				xwriter.WriteAttributeString("gain", this.GetGain(i).ToString());
				xwriter.WriteAttributeString("delay", this.GetDelay(i).ToString());
				FilterGenerator gen = this.GetSubfilter(i);
				gen.ToXml(xwriter);
				xwriter.WriteEndElement();
			}

			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			foreach(XmlNode node in elem.ChildNodes)
			{
				if(node is XmlElement)
				{
					double gain = this.converter.ToPower(node.Attributes["gain"].Value);
					int delay = int.Parse(node.Attributes["delay"].Value);
					FilterGenerator gen = FilterGenerator.CreateFromXml((XmlElement)node.FirstChild, this.converter);
					this.Add(gen, gain, delay);
				}
			}
		}

		public FilterGenerator GetSubfilter(int i)
		{
			return (FilterGenerator)this.arrayProperties[0][i, 0];
		}

		public double GetGain(int i)
		{
			return (double)this.arrayProperties[0][i, 1];
		}

		public int GetDelay(int i)
		{
			return (int)this.arrayProperties[0][i, 2];
		}

		public void SetSubfilter(int i, FilterGenerator gen)
		{
			this.arrayProperties[0][i, 0] = gen;
		}

		public int Length
		{
			get{return this.arrayProperties[0].ListLength;}
		}

		public void Add(FilterGenerator gen, double gain, int delay)
		{
			this.arrayProperties[0].Add(gen, gain, delay);
		}

		public void Remove(int i)
		{
			this.arrayProperties[0].Remove(i);
		}
	}//class DelayMixerGenerator
}
