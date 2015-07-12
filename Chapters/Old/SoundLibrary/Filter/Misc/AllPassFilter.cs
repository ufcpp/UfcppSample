using System;

namespace SoundLibrary.Filter.Misc
{
	using IDelay = SoundLibrary.Filter.Delay.IDelay;
	using Delay = SoundLibrary.Filter.Delay.Delay;
	using FractionalDelay = SoundLibrary.Filter.Delay.FractionalDelay;

	/// <summary>
		/// オールパスフィルタ。
		/// </summary>
	public class AllPassFilter : IFilter
	{
		double feedback;
		IDelay delay;

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="direct">ダイレクトゲイン</param>
		/// <param name="effect">エフェクトゲイン</param>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="delay">ディレイタイム</param>
		public AllPassFilter(double feedback, double delay)
		{
			--delay;
			if(delay < 0) delay = 0;

			this.feedback = feedback;
			this.delay = new FractionalDelay(delay);
			this.Clear();
		}

		public AllPassFilter(double feedback, double delay, int firLength)
		{
			--delay;
			if(delay < 0) delay = 0;

			this.feedback = feedback;
			this.delay = new FractionalDelay(delay, firLength);
			this.Clear();
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="delay">遅延時間</param>
		public void SetParameter(double feedback, double delay)
		{
			this.FeedbackGain = feedback;
			this.DelayTime    = delay;
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
			get{return this.delay.DelayTime + 1;}
			set
			{
				--value;
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
			double y = x + t * this.feedback;
			this.delay.Push(y);
				
			return y * -this.feedback + t;
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
			return new AllPassFilter(this.feedback, this.DelayTime);
		}

		public double GetBufferValue(int n)
		{
			return this.delay.GetBufferValue(n);
		}
	}//class AllPassFilter
}
