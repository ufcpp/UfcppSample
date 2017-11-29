using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// FIR を作成する。
	/// </summary>
	public class FirFilterGenerator : FilterGenerator
	{
		const string FilterName = "FIR フィルタ";
		const string CoefName   = "フィルタ係数";

		public FirFilterGenerator()
		{
			this.properties = new FilterProperty[1];

			double[] coef = new double[1];
			coef[0] = 1;

			this.properties[0] = new FilterProperty(CoefName, coef);
		}

		public double[] Coef
		{
			get{return (double[])this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public override string CheckConstraint()
		{
			return null;
		}

		public override IFilter GetFilter()
		{
			return new FirFilter(this.Coef);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("FirFilter");
			xwriter.WriteElementString("Coef", Util.ArrayToString(this.Coef));
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.Coef = Util.StringToArray(elem["Coef"].InnerText);
		}
	}//class FirFilterGenerator
}
