using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// 周波数系列(Middle/Side 形式)でデータを保持。
	/// </summary>
	public class WaveMS : WaveData
	{
		Spectrum m; // M (L + R) ch 周波数系列
		Spectrum s; // S (L - R) ch 周波数系列

		public WaveMS(){}

		public WaveMS(FormatHeader header, Spectrum m, Spectrum s) : base(header)
		{
			this.m = m;
			this.s = s;
		}

		public override double[] TimeL
		{
			set{this.Left = Spectrum.FromTimeSequence(value);}
			get{return this.Left.TimeSequence;}
		}

		public override double[] TimeR
		{
			set{this.Right = Spectrum.FromTimeSequence(value);}
			get{return this.Right.TimeSequence;}
		}

		public override int TimeLength
		{
			get
			{
				return this.m.TimeLength;
			}
		}

		public override Spectrum Left
		{
			set{this.SetLR(value, this.Right);}
			get{return 0.5 * (this.m + this.s);}
		}

		public override Spectrum Right
		{
			set{this.SetLR(this.Left, value);}
			get{return 0.5 * (this.m - this.s);}
		}

		public override Spectrum Middle
		{
			get{return this.m;}
			set{this.m = value;}
		}

		public override Spectrum Side
		{
			get{return this.s;}
			set{this.s = value;}
		}

		public override void SetLR(Spectrum left, Spectrum right)
		{
			this.m = left + right;
			this.s = left - right;
		}

		public override void SetMS(Spectrum middle, Spectrum side)
		{
			this.m = middle;
			this.s = side;
		}
		public override void Mul(Spectrum s)
		{
			this.m *= s;
			this.s *= s;
		}

		public override void Div(Spectrum s)
		{
			this.m /= s;
			this.s /= s;
		}
	}
}
