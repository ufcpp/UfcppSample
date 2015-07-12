using System;

namespace SoundLibrary.Filter.Misc
{
	using IDelay = SoundLibrary.Filter.Delay.IDelay;
	using Delay = SoundLibrary.Filter.Delay.Delay;
	using FractionalDelay = SoundLibrary.Filter.Delay.FractionalDelay;

	/// <summary>
	/// コムフィルタ。
	/// </summary>
	public class CombFilter : IFilter
	{
		double direct;
		double effect;
		double feedback;
		IDelay delay;

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="direct">ダイレクトゲイン</param>
		/// <param name="effect">エフェクトゲイン</param>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="delay">ディレイタイム</param>
		public CombFilter(double direct, double effect, double feedback, double delayTime)
			: this(direct, effect, feedback, delayTime, new FractionalDelay(delayTime))
		{
		}

		public CombFilter(double direct, double effect, double feedback, double delayTime, IDelay delay)
		{
			this.direct = direct;
			this.effect = effect;
			this.feedback = feedback;
			this.delay = delay;
			this.DelayTime = delayTime;
			this.Clear();
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="direct">ダイレクトゲイン</param>
		/// <param name="effect">エフェクトゲイン</param>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="delay">遅延時間</param>
		public void SetParameter(double direct, double effect, double feedback, double delay)
		{
			this.DirectGain   = direct;
			this.EffectGain   = effect;
			this.FeedbackGain = feedback;
			this.DelayTime    = delay;
		}

		/// <summary>
		/// ダイレクトゲイン
		/// </summary>
		public double DirectGain
		{
			get{return this.direct;}
			set{this.direct = value;}
		}

		/// <summary>
		/// エフェクトゲイン
		/// </summary>
		public double EffectGain
		{
			get{return this.effect;}
			set{this.effect = value;}
		}

		/// <summary>
		/// フィードバックゲイン
		/// </summary>
		public double FeedbackGain
		{
			get{return this.feedback;}
			set{this.feedback = value;}
		}


		/// <summary>
		/// ディレイタイム
		/// </summary>
		public double DelayTime
		{
			get{return this.delay.DelayTime;}
			set
			{
				if(value < 0) value = 0;
				this.delay.DelayTime = value;
			}
		}

		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			double t = this.delay.GetValue();
			double y = x * this.direct + t * this.effect;
			this.delay.Push(x + t * this.feedback);
			return y;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			this.delay.Clear();
		}

		public object Clone()
		{
			return new CombFilter(this.direct, this.effect, this.feedback, this.DelayTime);
		}

		public double GetBufferValue(int n)
		{
			return this.delay.GetBufferValue(n);
		}
	}
}
