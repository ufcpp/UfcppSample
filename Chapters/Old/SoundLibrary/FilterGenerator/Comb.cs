using System;
using System.Xml;

namespace SoundLibrary.Filter
{
	using CombFilter = SoundLibrary.Filter.Misc.CombFilter;

	/// <summary>
	/// CombFilter を作成する。
	/// </summary>
	public class CombFilterGenerator : FilterGenerator
	{
		const string FilterName = "コムフィルタ";
		const string DirectGainName = "ダイレクト増幅率";
		const string EffectGainName = "エフェクト増幅率";
		const string FeedbackName   = "フィードバック増幅率";
		const string DelayName  = "遅延サンプル数";

		public CombFilterGenerator()
		{
			this.properties = new FilterProperty[4];
			this.properties[0] = new FilterProperty(DirectGainName, (double)0.0);
			this.properties[1] = new FilterProperty(EffectGainName, (double)0.0);
			this.properties[2] = new FilterProperty(FeedbackName, (double)0.0);
			this.properties[3] = new FilterProperty(DelayName, (int)0);
		}

		public double DirectGain
		{
			get{return (double)this.properties[0].Value;}
			set{this.properties[0].Value = value;}
		}

		public double EffectGain
		{
			get{return (double)this.properties[1].Value;}
			set{this.properties[1].Value = value;}
		}

		public double FeedbackGain
		{
			get{return (double)this.properties[2].Value;}
			set{this.properties[2].Value = value;}
		}

		public int Delay
		{
			get{return (int)this.properties[3].Value;}
			set{this.properties[3].Value = value;}
		}

		public override string CheckConstraint()
		{
			return null;
		}

		public override IFilter GetFilter()
		{
			return new CombFilter(this.DirectGain, this.EffectGain, this.FeedbackGain, this.Delay);
		}

		public override void ToXml(XmlWriter xwriter)
		{
			xwriter.WriteStartElement("CombFilter");
			xwriter.WriteAttributeString("direct", this.DirectGain.ToString());
			xwriter.WriteAttributeString("effect", this.EffectGain.ToString());
			xwriter.WriteAttributeString("feedback", this.FeedbackGain.ToString());
			xwriter.WriteAttributeString("delay", this.Delay.ToString());
			xwriter.WriteEndElement();
		}

		public override void FromXml(XmlElement elem)
		{
			this.DirectGain = double.Parse(elem.Attributes["direct"].Value);
			this.EffectGain = double.Parse(elem.Attributes["effect"].Value);
			this.FeedbackGain = double.Parse(elem.Attributes["feedback"].Value);
			this.Delay = int.Parse(elem.Attributes["delay"].Value);
		}
	}//class CombFilterGenerator
}
