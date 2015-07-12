using System;

using SoundLibrary.SpectrumAnalysis;
using SoundLibrary.Wave;

namespace SoundLibrary.WaveAnalysis
{
	/// <summary>
	/// Wave データ格納用クラス。
	/// 効率を考えて、時系列のままデータを保持しておく WaveTime と、
	/// 周波数領域に変換してデータを保持しておく WaveFrequency と、
	/// Middle/Side 形式でデータを保持しておく WaveMS に分ける。
	/// このクラスは抽象基底クラス。
	/// </summary>
	public abstract class WaveData
	{
		FormatHeader header;

		#region コンストラクタ

		public WaveData(){}

		public WaveData(FormatHeader header)
		{
			this.header = header;
		}

		#endregion
		#region Wave ヘッダ・時系列データの取得

		/// <summary>
		/// Wave ヘッダを取得。
		/// </summary>
		public FormatHeader Header{get{return this.header;}}

		/// <summary>
		/// 時系列 L ch 信号を取得。
		/// </summary>
		public abstract double[] TimeL{set; get;}

		/// <summary>
		/// 時系列 R ch 信号を取得。
		/// </summary>
		public abstract double[] TimeR{set; get;}

		/// <summary>
		/// 時系列の長さ。
		/// </summary>
		public virtual int TimeLength
		{
			get{return this.TimeL.Length;}
		}

		#endregion
		#region 周波数特性の取得・設定

		/// <summary>
		/// 周波数特性 L ch 信号を取得。
		/// </summary>
		public abstract Spectrum Left{set; get;}

		/// <summary>
		/// 周波数特性 R ch 信号を取得。
		/// </summary>
		public abstract Spectrum Right{set; get;}

		/// <summary>
		/// 周波数特性 Middle (L + R) ch 信号を取得。
		/// </summary>
		public virtual Spectrum Middle
		{
			get{return this.Left + this.Right;}
			set{this.SetMS(value, this.Side);}
		}

		/// <summary>
		/// 周波数特性 Side (L - R) ch 信号を取得。
		/// </summary>
		public virtual Spectrum Side
		{
			get{return this.Left - this.Right;}
			set{this.SetMS(this.Middle, value);}
		}

		/// <summary>
		/// Left/Right ch 信号を設定。
		/// </summary>
		/// <param name="middle">M ch</param>
		/// <param name="side">S ch</param>
		public virtual void SetLR(Spectrum left, Spectrum right)
		{
			this.Left = left;
			this.Right = right;
		}

		/// <summary>
		/// Middle/Side ch 信号を設定。
		/// </summary>
		/// <param name="middle">M ch</param>
		/// <param name="side">S ch</param>
		public virtual void SetMS(Spectrum middle, Spectrum side)
		{
			this.Left  = 0.5 * (middle + side);
			this.Right = 0.5 * (middle - side);
		}

		/// <summary>
		/// 周波数特性の長さ。
		/// </summary>
		public virtual int Count
		{
			get{return this.Left.Count;}
		}

		#endregion
		#region 内部形式の変換

		/// <summary>
		/// 内部形式を時系列に変換。
		/// </summary>
		/// <returns>内部形式を時系列で持つ WaveData</returns>
		public WaveTime ToTime()
		{
			return new WaveTime(this.header, this.TimeL, this.TimeR);
		}

		/// <summary>
		/// 内部形式を周波数特性に変換。
		/// </summary>
		/// <returns>内部形式を周波数特性で持つ WaveData</returns>
		public WaveFrequency ToSpectrum()
		{
			return new WaveFrequency(this.header, this.Left, this.Right);
		}

		/// <summary>
		/// 内部形式を周波数特性(Middle/Side)に変換。
		/// </summary>
		/// <returns>内部形式を周波数特性で持つ WaveData</returns>
		public WaveMS ToMS()
		{
			return new WaveMS(this.header, this.Middle, this.Side);
		}

		#endregion
		#region 周波数特性の解析・特性調整など

		/// <summary>
		/// 左右の時間差を取得。
		/// 正: Left ch の方が遅い。
		/// 負: Right ch の方が遅い。
		/// </summary>
		/// <returns>左右の時間差</returns>
		public int GetDelay()
		{
			return SoundLibrary.Mathematics.Discrete.Function.Argmax(
				new SoundLibrary.Mathematics.Discrete.Correlation(this.TimeL, this.TimeR));
		}

		/// <summary>
		/// 遅延を与える。
		/// </summary>
		/// <param name="delay">遅延時間</param>
		/// <returns>遅延を与えた後のデータ</returns>
		public WaveData AddDelay(int delay)
		{
			WaveTime w = this.ToTime();
			double[] l = w.TimeL;
			double[] r = w.TimeR;

			int i = l.Length - 1;
			int n = i - delay;
			for(; n>=0; --i, --n)
			{
				l[i] = l[n];
				r[i] = r[n];
			}
			for(; i>=0; --i)
			{
				l[i] = 0;
				r[i] = 0;
			}
			return w;
		}

		/// <summary>
		/// 最小位相化する。
		/// 左右の信号の遅延差も付加。
		/// </summary>
		/// <param name="baseDelay">L/R 両方にかける遅延</param>
		/// <returns>最小位相化したのデータ</returns>
		public WaveData ConvertToMinimumPhase(int baseDelay)
		{
			int delay = this.GetDelay();

			WaveFrequency w = this.ToSpectrum();
			w.Left.ConvertToMinimumPhase();
			w.Right.ConvertToMinimumPhase();

			w.Left  *= Spectrum.FromDelay(baseDelay + delay/2, this.Left.TimeLength);
			w.Right *= Spectrum.FromDelay(baseDelay - delay/2, this.Right.TimeLength);

			return w;
		}

		/// <summary>
		/// 最小位相化する。
		/// 左右の信号の遅延差も付加。
		/// </summary>
		public WaveData ConvertToMinimumPhase()
		{
			return this.ConvertToMinimumPhase(32);
		}

		/// <summary>
		/// F = this<br />
		/// [Gl Gr]   [Fl Fr]^-1<br />
		/// [Gr Gl] = [Fr Fl]   <br />
		/// G を求める。
		/// </summary>
		/// <returns>G</returns>
		public WaveData Invert()
		{
			return new WaveMS(this.header, this.Middle.Invert(), this.Side.Invert());
		}

		/// <summary>
		/// Al = a.Left, Ar = a.Right<br />
		/// Bl = b.Left, Br = b.Right<br />
		/// Cl = c.Left, Cr = c.Right<br />
		/// [Cl]   [Bl Br][Al]   [Al Ar][Bl]<br />
		/// [Cr] = [Br Bl][Ar] = [Ar Al][Br]<br />
		/// 
		/// c を求める。
		/// c.Middle = a.Middle * b.Middle,
		/// c.Side = a.Side * b.Side
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド1</param>
		/// <returns>計算結果</returns>
		public static WaveData operator* (WaveData a, WaveData b)
		{
			return new WaveMS(a.header, a.Middle * b.Middle, a.Side * b.Side);
		}

		/// <summary>
		/// Al = a.Left, Ar = a.Right<br />
		/// Bl = b.Left, Br = b.Right<br />
		/// Cl = c.Left, Cr = c.Right<br />
		/// [Cl]   [Bl Br]^-1[Al]<br />
		/// [Cr] = [Br Bl]   [Ar]<br />
		/// 
		/// c を求める。
		/// c.Middle = a.Middle / b.Middle,
		/// c.Side = a.Side / b.Side
		/// </summary>
		/// <param name="a">オペランド1</param>
		/// <param name="b">オペランド1</param>
		/// <returns>計算結果</returns>
		public static WaveData operator/ (WaveData a, WaveData b)
		{
			return new WaveMS(a.header, a.Middle / b.Middle, a.Side / b.Side);
		}

		/// <summary>
		/// Left/Right に s を掛ける。
		/// </summary>
		/// <param name="s">周波数特性</param>
		public virtual void Mul(Spectrum s)
		{
			this.Left *= s;
			this.Right *= s;
		}

		/// <summary>
		/// Left/Right を s で割る。
		/// </summary>
		/// <param name="s">周波数特性</param>
		public virtual void Div(Spectrum s)
		{
			this.Left /= s;
			this.Right /= s;
		}

		/// <summary>
		/// b.Left = a.Left * s, b.Right = a.Right * s;
		/// </summary>
		/// <param name="a">被乗数</param>
		/// <param name="s">乗数</param>
		/// <returns>乗算結果</returns>
		public static WaveData operator* (WaveData a, Spectrum s)
		{
			WaveData b = a.ToSpectrum();
			b.Mul(s);
			return b;
		}

		/// <summary>
		/// b.Left = a.Left / s, b.Right = a.Right / s;
		/// </summary>
		/// <param name="a">被除数</param>
		/// <param name="s">除数</param>
		/// <returns>除算結果</returns>
		public static WaveData operator/ (WaveData a, Spectrum s)
		{
			WaveData b = a.ToSpectrum();
			b.Div(s);
			return b;
		}

		#endregion
		#region ↓あんまりいらない気がする。
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
				/*
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
				*/
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
		#endregion
	}//class Wave
}//namespace WaveAnalysis
