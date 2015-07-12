using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// ShelvingEqualizer を作成する。
	/// </summary>
	public class ShelvingEqualizerGenerator : FilterGenerator
	{
		const string FilterName = "低域透過フィルタ";
		const string CenterName = "中心周波数";
		const string QName = "Q値";
		const string GainName = "増幅率";

		public ShelvingEqualizerGenerator()
		{
			this.properties = new FilterProperty[2];
			this.properties[0] = new FilterProperty(CenterName, (double)0);
			this.properties[1] = new FilterProperty(GainName, (double)1);
		}

		public double CenterFrequency
		{
			get{return (double)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double Gain
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public override string CheckConstraint()
		{
			double center = this.CenterFrequency;
			if(center < 0 || center > Math.PI)
				return "中心周波数は 0 ～ π の間でなければなりません。";

			return null;
		}

		public override IFilter GetFilter()
		{
			return new ShelvingEqualizer(this.CenterFrequency, this.Gain);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("ShelvingEqualizer");
			xwriter.WriteAttributeString("center", this.CenterFrequency.ToString());
			xwriter.WriteAttributeString("gain"  , this.Gain           .ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.CenterFrequency = this.converter.ToFrequency(elem.Attributes["center"].Value);
			this.Gain            = this.converter.ToPower(elem.Attributes["gain"  ].Value);
		}
	}//class ShelvingEqualizerGenerator

	/// <summary>
	/// ShelvingEqualizer を作成する。
	/// </summary>
	public class PeakingEqualizerGenerator : FilterGenerator
	{
		const string FilterName = "低域透過フィルタ";
		const string CenterName = "中心周波数";
		const string QName = "Q値";
		const string GainName = "増幅率";

		public PeakingEqualizerGenerator()
		{
			this.properties = new FilterProperty[3];
			this.properties[0] = new FilterProperty(CenterName, (double)0);
			this.properties[1] = new FilterProperty(QName, (double)1);
			this.properties[2] = new FilterProperty(GainName, (double)1);
		}

		public double CenterFrequency
		{
			get{return (double)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double Q
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public double Gain
		{
			get{return (double)this.properties[2].Value;}
			set{this.properties[2].Value = value;}
		}

		public override string CheckConstraint()
		{
			double center = this.CenterFrequency;
			if(center < 0 || center > Math.PI)
				return "中心周波数は 0 ～ π の間でなければなりません。";

			if(this.Q == 0)
				return "Q値は非0でなければなりません。";

			return null;
		}

		public override IFilter GetFilter()
		{
			return new PeakingEqualizer(this.CenterFrequency, this.Q, this.Gain);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("PeakingEqualizer");
			xwriter.WriteAttributeString("center", this.CenterFrequency.ToString());
			xwriter.WriteAttributeString("q"     , this.Q              .ToString());
			xwriter.WriteAttributeString("gain"  , this.Gain           .ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.CenterFrequency = this.converter.ToFrequency(elem.Attributes["center"].Value);
			this.Q               = double.Parse(elem.Attributes["q"     ].Value);
			this.Gain            = this.converter.ToPower(elem.Attributes["gain"  ].Value);
		}
	}//class PeakingEqualizerGenerator
}
