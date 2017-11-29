using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// Low Pass FIR を作成する。
	/// </summary>
	public class LowPassFirGenerator : FilterGenerator
	{
		const string FilterName = "低域透過フィルタ";
		const string CutoffName = "カットオフ周波数";
		const string OrderName  = "フィルタ次数";
		const string TypeName   = "窓関数タイプ";

		public LowPassFirGenerator()
		{
			this.properties = new FilterProperty[3];
			this.properties[0] = new FilterProperty(OrderName, 1);
			this.properties[1] = new FilterProperty(CutoffName, (double)0);
			this.properties[2] = new FilterProperty(TypeName, WindowType.Rectangular);
		}

		public int Order
		{
			get{return (int)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double Cutoff
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public WindowType WindowType
		{
			get{return (WindowType)this.properties[2].Value;}
			set{this.properties[2].Value = value;}
		}

		public override string CheckConstraint()
		{
			if(this.Order <= 0)
				return "次数は 1 以上でなければなりません。";

			double cutoff = this.Cutoff;
			if(cutoff < 0 || cutoff > Math.PI)
				return "カットオフ周波数は 0 ～ π の間でなければなりません。";

			return null;
		}

		public override IFilter GetFilter()
		{
			return FirCommon.GetLowPassFilter(this.Order, this.Cutoff, this.WindowType);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("LowPassFir");
			xwriter.WriteAttributeString("order",  this.Order     .ToString());
			xwriter.WriteAttributeString("cutoff", this.Cutoff    .ToString());
			xwriter.WriteAttributeString("window", this.WindowType.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Order      = int.Parse(elem.Attributes["order"].Value);
			this.Cutoff     = this.converter.ToFrequency(elem.Attributes["cutoff"].Value);
			this.WindowType = (WindowType)Enum.Parse(typeof(WindowType), elem.Attributes["window"].Value);
		}
	}//class LowPassFirGenerator

	/// <summary>
	/// High Pass FIR を作成する。
	/// </summary>
	public class HighPassFirGenerator : FilterGenerator
	{
		const string FilterName = "高域透過フィルタ";
		const string CutoffName = "カットオフ周波数";
		const string OrderName  = "フィルタ次数";
		const string TypeName   = "窓関数タイプ";

		public HighPassFirGenerator()
		{
			this.properties = new FilterProperty[3];
			this.properties[0] = new FilterProperty(OrderName, 1);
			this.properties[1] = new FilterProperty(CutoffName, (double)0);
			this.properties[2] = new FilterProperty(TypeName, WindowType.Rectangular);
		}

		public int Order
		{
			get{return (int)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double Cutoff
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public WindowType WindowType
		{
			get{return (WindowType)this.properties[2].Value;}
			set{this.properties[2].Value = value;}
		}

		public override string CheckConstraint()
		{
			if(this.Order <= 0)
				return "次数は 1 以上でなければなりません。";

			double cutoff = this.Cutoff;
			if(cutoff < 0 || cutoff > Math.PI)
				return "カットオフ周波数は 0 ～ π の間でなければなりません。";

			return null;
		}

		public override IFilter GetFilter()
		{
			return FirCommon.GetHighPassFilter(this.Order, this.Cutoff, this.WindowType);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("HighPassFir");
			xwriter.WriteAttributeString("order",  this.Order     .ToString());
			xwriter.WriteAttributeString("cutoff", this.Cutoff    .ToString());
			xwriter.WriteAttributeString("window", this.WindowType.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Order      = int.Parse(elem.Attributes["order"].Value);
			this.Cutoff     = this.converter.ToFrequency(elem.Attributes["cutoff"].Value);
			this.WindowType = (WindowType)Enum.Parse(typeof(WindowType), elem.Attributes["window"].Value);
		}
	}//class HighPassFirGenerator

	/// <summary>
	/// Band Pass FIR を作成する。
	/// </summary>
	public class BandPassFirGenerator : FilterGenerator
	{
		const string FilterName = "帯域透過フィルタ";
		const string UpperName = "上限周波数";
		const string LowerfName = "下限周波数";
		const string OrderName  = "フィルタ次数";
		const string TypeName   = "窓関数タイプ";

		public BandPassFirGenerator()
		{
			this.properties = new FilterProperty[4];
			this.properties[0] = new FilterProperty(OrderName, 1);
			this.properties[1] = new FilterProperty(UpperName, (double)0);
			this.properties[2] = new FilterProperty(LowerfName, (double)0);
			this.properties[3] = new FilterProperty(TypeName, WindowType.Rectangular);
		}

		public int Order
		{
			get{return (int)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double UpperBound
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public double LowerBound
		{
			get{return (double)this.properties[2].Value;}
			set{this.properties[2].Value = value;}
		}

		public WindowType WindowType
		{
			get{return (WindowType)this.properties[3].Value;}
			set{this.properties[3].Value = value;}
		}

		public override string CheckConstraint()
		{
			if(this.Order <= 0)
				return "次数は 1 以上でなければなりません。";

			double upper = this.UpperBound;
			if(upper < 0 || upper > Math.PI)
				return "上限周波数は 0 ～ π の間でなければなりません。";

			double lower = this.LowerBound;
			if(lower < 0 || lower > Math.PI)
				return "下限周波数は 0 ～ π の間でなければなりません。";

			if(lower > upper)
				return "下限が上限を超えています。";

				return null;
		}

		public override IFilter GetFilter()
		{
			return FirCommon.GetBandPassFilter(this.Order, this.LowerBound, this.UpperBound, this.WindowType);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("BandPassFir");
			xwriter.WriteAttributeString("order",      this.Order.ToString());
			xwriter.WriteAttributeString("lowerBound", this.LowerBound.ToString());
			xwriter.WriteAttributeString("upperBound", this.UpperBound.ToString());
			xwriter.WriteAttributeString("window",     this.WindowType.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Order      = int.Parse(elem.Attributes["order"].Value);
			this.LowerBound = this.converter.ToFrequency(elem.Attributes["lowerBound"].Value);
			this.UpperBound = this.converter.ToFrequency(elem.Attributes["upperBound"].Value);
			this.WindowType = (WindowType)Enum.Parse(typeof(WindowType), elem.Attributes["window"].Value);
		}
	}//class BandPassFirGenerator
}
