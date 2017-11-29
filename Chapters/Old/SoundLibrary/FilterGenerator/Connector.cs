using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// SerialConnector を作成する。
	/// </summary>
	public class SerialConnectorGenerator : FilterGenerator
	{
		const string FilterName = "直列接続";
		const string SubFilterName = "フィルタ";

		public SerialConnectorGenerator()
		{
			this.arrayProperties = new FilterArrayProperty[1];
			this.arrayProperties[0] = new FilterArrayProperty(
				new FilterArrayProperty.Tuple(SubFilterName, typeof(FilterGenerator))
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
			IFilter[] filters = new IFilter[len];
			for(int i=0; i<len; ++i)
			{
				FilterGenerator gen = this.GetSubfilter(i);
				filters[i] = gen.GetFilter();
			}
			return new SerialConnector(filters);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("SerialConnector");

			int len = this.Length;
			for(int i=0; i<len; ++i)
			{
				FilterGenerator gen = this.GetSubfilter(i);
				gen.ToXml(xwriter);
			}

			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			foreach(XmlNode node in elem.ChildNodes)
			{
				if(node is XmlElement)
				{
					FilterGenerator gen = FilterGenerator.CreateFromXml((XmlElement)node, this.converter);
					this.Add(gen);
				}
			}
		}

		public FilterGenerator GetSubfilter(int i)
		{
			return (FilterGenerator)this.arrayProperties[0][i, 0];
		}

		public void SetSubfilter(int i, FilterGenerator gen)
		{
			this.arrayProperties[0][i, 0] = gen;
		}

		public int Length
		{
			get{return this.arrayProperties[0].ListLength;}
		}

		public void Add(FilterGenerator gen)
		{
			this.arrayProperties[0].Add(gen);
		}

		public void Remove(int i)
		{
			this.arrayProperties[0].Remove(i);
		}
	}//class SerialConnectorGenerator

	/// <summary>
	/// PallarelConnector を作成する。
	/// </summary>
	public class PallarelConnectorGenerator : FilterGenerator
	{
		const string FilterName = "並列接続";
		const string SubFilterName = "フィルタ";

		public PallarelConnectorGenerator()
		{
			this.arrayProperties = new FilterArrayProperty[1];
			this.arrayProperties[0] = new FilterArrayProperty(
				new FilterArrayProperty.Tuple(SubFilterName, typeof(FilterGenerator))
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
			IFilter[] filters = new IFilter[len];
			for(int i=0; i<len; ++i)
			{
				FilterGenerator gen = this.GetSubfilter(i);
				filters[i] = gen.GetFilter();
			}
			return new PallarelConnector(filters);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("PallarelConnector");

			int len = this.Length;
			for(int i=0; i<len; ++i)
			{
				FilterGenerator gen = this.GetSubfilter(i);
				gen.ToXml(xwriter);
			}

			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			foreach(XmlNode node in elem.ChildNodes)
			{
				if(node is XmlElement)
				{
					FilterGenerator gen = FilterGenerator.CreateFromXml((XmlElement)node, this.converter);
					this.Add(gen);
				}
			}
		}

		public FilterGenerator GetSubfilter(int i)
		{
			return (FilterGenerator)this.arrayProperties[0][i, 0];
		}

		public void SetSubfilter(int i, FilterGenerator gen)
		{
			this.arrayProperties[0][i, 0] = gen;
		}

		public int Length
		{
			get{return this.arrayProperties[0].ListLength;}
		}

		public void Add(FilterGenerator gen)
		{
			this.arrayProperties[0].Add(gen);
		}

		public void Remove(int i)
		{
			this.arrayProperties[0].Remove(i);
		}
	}//class PallarelConnectorGenerator
}
