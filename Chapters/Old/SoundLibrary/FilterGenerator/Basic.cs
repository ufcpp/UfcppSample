using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// 増幅器を作成する。
	/// </summary>
	public class AmplifierGenerator : FilterGenerator
	{
		const string FilterName = "増幅器";
		const string GainName   = "増幅率";

		public AmplifierGenerator()
		{
			this.properties = new FilterProperty[1];
			this.properties[0] = new FilterProperty(GainName, (double)1.0);
		}

		public override string CheckConstraint()
		{
			return null;
		}

		public override IFilter GetFilter()
		{
			double gain = this.Gain;
			return new Misc.Amplifier(gain);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("Amplifier");
			xwriter.WriteAttributeString("gain", this.Gain.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Gain = this.converter.ToPower(elem.Attributes["gain"].Value);
		}

		public double Gain
		{
			get{return (double)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}
	}//class AmplifierGenerator

	/// <summary>
	/// 遅延器を作成する。
	/// </summary>
	public class DelayGenerator : FilterGenerator
	{
		const string FilterName = "遅延器";
		const string DelayName  = "遅延サンプル数";

		public DelayGenerator()
		{
			this.properties = new FilterProperty[1];
			this.properties[0] = new FilterProperty(DelayName, (int)0);
		}

		public override string CheckConstraint()
		{
			return null;
		}

		public override IFilter GetFilter()
		{
			int delay = this.Delay;
			return new Delay.Delay(delay);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("Delay");
			xwriter.WriteAttributeString("delay", this.Delay.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Delay = int.Parse(elem.Attributes["delay"].Value);
		}

		public int Delay
		{
			get{return (int)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}
	}//class DelayGenerator

	/// <summary>
	/// MultiDelay を作成する。
	/// </summary>
	public class MultiDelayGenerator : FilterGenerator
	{
		const string FilterName = "マルチディレイ";
		const string GainName   = "増幅率";
		const string DelayName  = "遅延サンプル数";

		public MultiDelayGenerator()
		{
			this.arrayProperties = new FilterArrayProperty[1];
			this.arrayProperties[0] = new FilterArrayProperty(
				new FilterArrayProperty.Tuple(GainName, typeof(double)),
				new FilterArrayProperty.Tuple(DelayName, typeof(int))
				);
		}

		public override string CheckConstraint()
		{
			return null;
		}

		public override IFilter GetFilter()
		{
			int len = this.Length;
			Misc.MultiDelay.Tuple[] tuples = new Misc.MultiDelay.Tuple[len];
			for(int i=0; i<len; ++i)
			{
				double gain = this.GetGain(i);
				int delay = this.GetDelay(i);
				tuples[i] = new Misc.MultiDelay.Tuple(gain, delay);
			}
			return new Misc.MultiDelay(tuples);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("MultiDelay");

			int len = this.Length;
			for(int i=0; i<len; ++i)
			{
				xwriter.WriteStartElement("Filter");
				xwriter.WriteAttributeString("gain", this.GetGain(i).ToString());
				xwriter.WriteAttributeString("delay", this.GetDelay(i).ToString());
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
					this.Add(gain, delay);
				}
			}
		}

		public double GetGain(int i)
		{
			return (double)this.arrayProperties[0][i, 0];
		}

		public int GetDelay(int i)
		{
			return (int)this.arrayProperties[0][i, 1];
		}

		public void SetSubfilter(int i, FilterGenerator gen)
		{
			this.arrayProperties[0][i, 0] = gen;
		}

		public int Length
		{
			get{return this.arrayProperties[0].ListLength;}
		}

		public void Add(double gain, int delay)
		{
			this.arrayProperties[0].Add(gain, delay);
		}

		public void Remove(int i)
		{
			this.arrayProperties[0].Remove(i);
		}
	}//class MultiDelayGenerator
}
