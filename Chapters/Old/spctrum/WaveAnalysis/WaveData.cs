using System;

using SpectrumAnalysis;
using Wave;

namespace WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// </summary>
	public abstract class WaveData
	{
		FormatHeader header;

		public WaveData(){}
		public WaveData(FormatHeader header)
		{
			this.header = header;
		}

		public FormatHeader Header{get{return this.header;}}

		public abstract double[] TimeL{set; get;}
		public abstract double[] TimeR{set; get;}
		public abstract Spectrum Left{set; get;}
		public abstract Spectrum Right{set; get;}

		public Spectrum Middle
		{
			get{return this.Left + this.Right;}
		}

		public Spectrum Side
		{
			get{return this.Left - this.Right;}
		}

		public WaveData ToTime()
		{
			return new WaveTime(this.header, this.TimeL, this.TimeR);
		}

		public WaveData ToSpectrum()
		{
			return new WaveFrequency(this.header, this.Left, this.Right);
		}

		public int Count
		{
			get{return this.Left.Count - 1;}
		}

		/// <summary>
		/// データの特性を取得。
		/// </summary>
		/// <param name="spectrum">スペクトル</param>
		/// <param name="type">特性の種類</param>
		/// <returns>特性</returns>
		public static double[] GetData(Spectrum spectrum, Property type)
		{
			switch(type)
			{
				case Property.Amplitude:
				{
					double[] tmp = spectrum.GetPower();
					Spectrum.Smooth(tmp);
					return tmp;
				}
				case Property.Phase:
				{
					double[] tmp = spectrum.GetPhase();
					Spectrum.Unwrap(tmp);
					Spectrum.Smooth(tmp);
					return tmp;
				}
				case Property.MinimumPhase:
				{
					double[] tmp = spectrum.GetMinimumPhase();
					Spectrum.Smooth(tmp);
					return tmp;
				}
				case Property.AllPassPhase:
				{
					double[] tmp  = spectrum.GetPhase();
					double[] tmp2 = spectrum.GetMinimumPhase();
					for(int i=0; i<tmp.Length; ++i) tmp[i] += tmp2[i];
					Spectrum.Unwrap(tmp);
					Spectrum.Smooth(tmp);
					return tmp;
				}
				case Property.PhaseDelay:
				{
					double[] tmp = spectrum.GetPhase();
					Spectrum.Unwrap(tmp);
					Spectrum.Smooth(tmp);
					return Spectrum.GetPhaseDalay(tmp, 48000);
				}
				case Property.GroupDelay:
				{
					double[] tmp = spectrum.GetPhase();
					Spectrum.Unwrap(tmp);
					Spectrum.Smooth(tmp);
					return Spectrum.GetGroupDalay(tmp, 48000);
				}
				default:
					return spectrum.TimeSequence;
			}
		}//GetData

		/// <summary>
		/// データの特性を取得。
		/// </summary>
		/// <param name="channel">取得したいチャネル</param>
		/// <param name="type">取得したい特性</param>
		/// <returns>特性</returns>
		public double[] GetData(Channel channel, Property type)
		{
			switch(channel)
			{
				case Channel.Left:   return GetData(this.Left, type);
				case Channel.Right:  return GetData(this.Right, type);
				case Channel.LR:     return GetData(this.Left / this.Right, type);
				case Channel.RL:     return GetData(this.Right / this.Left, type);
				case Channel.Middle: return GetData(this.Middle, type);
				case Channel.Side:   return GetData(this.Side, type);
				case Channel.MS:     return GetData(this.Middle / this.Side, type);
				case Channel.SM:     return GetData(this.Side / this.Middle, type);
				default: return null;
			}
		}//GetData

		/// <summary>
		/// スペクトルの取得。
		/// </summary>
		/// <param name="channel">取得したいチャネル</param>
		/// <returns>スペクトル</returns>
		public Spectrum GetSpectrum(Channel channel)
		{
			switch(channel)
			{
				case Channel.Left:   return this.Left;
				case Channel.Right:  return this.Right;
				case Channel.LR:     return this.Left / this.Right;
				case Channel.RL:     return this.Right / this.Left;
				case Channel.Middle: return this.Middle;
				case Channel.Side:   return this.Side;
				case Channel.MS:     return this.Middle / this.Side;
				case Channel.SM:     return this.Side / this.Middle;
				default: return null;
			}
		}//GetSpectrum
	}//class Wave

	/// <summary>
	/// Wave データ格納用クラス。
	/// 時系列でデータを保持。
	/// </summary>
	class WaveTime : WaveData
	{
		double[] l;  // L ch 時系列
		double[] r;  // R ch 時系列

		public WaveTime(){}

		public WaveTime(FormatHeader header, double[] l, double[] r) : base(header)
		{
			this.l = l;
			this.r = r;
		}

		public override double[] TimeL
		{
			set{this.l = value;}
			get{return this.l;}
		}

		public override double[] TimeR
		{
			set{this.r = value;}
			get{return this.r;}
		}

		public override Spectrum Left
		{
			set{this.l = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.l, this.Header.sampleRate);}
		}

		public override Spectrum Right
		{
			set{this.r = value.TimeSequence;}
			get{return Spectrum.FromTimeSequence(this.r, this.Header.sampleRate);}
		}
	}//class WaveTime

	/// <summary>
	/// チャネルのタイプ。
	/// </summary>
	public enum Channel
	{
		Left,   // L チャネル
		Right,  // R チャネル
		LR,     // Left / Right
		RL,     // Right / Left
		Middle, // M チャネル
		Side,   // S チャネル
		MS,     // Middle / Side
		SM,     // Side / Middle
	}

	/// <summary>
	/// 特性のタイプ。
	/// </summary>
	public enum Property
	{
		Amplitude,    // 振幅特性
		Phase,        // 位相特性
		PhaseDelay,   // 位相遅延特性
		GroupDelay,   // 群遅延特性
		MinimumPhase, // 最小位相特性
		AllPassPhase, // オールパス位相特性
		TimeSequence, // 時系列データ
	}
}//namespace WaveAnalysis
