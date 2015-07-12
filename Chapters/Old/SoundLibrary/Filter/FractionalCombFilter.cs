using System;

namespace SoundLibrary.Filter
{
	/// <summary>
	/// 分数遅延コムフィルタ。
	/// </summary>
	[Obsolete("SoundLibrary.Filter.Misc.CombFilter を使ってください。")]
	public class FractionalCombFilter : IFilter
	{
		double direct;
		double effect;
		double feedback;
		Delay.FractionalDelay delay;

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="direct">ダイレクトゲイン</param>
		/// <param name="effect">エフェクトゲイン</param>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="buffer">バッファサイズ</param>
		public FractionalCombFilter(double direct, double effect, double feedback, double buffer)
		{
			--buffer;
			if(buffer < 0) buffer = 0;

			this.direct = direct;
			this.effect = effect;
			this.feedback = feedback;
			this.delay = new Delay.FractionalDelay(buffer);
			this.Clear();
		}

		public FractionalCombFilter(double direct, double effect, double feedback, double buffer, int firLength)
		{
			--buffer;
			if(buffer < 0) buffer = 0;

			this.direct = direct;
			this.effect = effect;
			this.feedback = feedback;
			this.delay = new Delay.FractionalDelay(buffer, firLength);
			this.Clear();
		}

		/// <summary>
		/// パラメータを設定する。
		/// </summary>
		/// <param name="direct">ダイレクトゲイン</param>
		/// <param name="effect">エフェクトゲイン</param>
		/// <param name="feedback">フィードバックゲイン</param>
		/// <param name="buffer">遅延時間</param>
		public void SetParameter(double direct, double effect, double feedback, double buffer)
		{
			this.DirectGain   = direct;
			this.EffectGain   = effect;
			this.FeedbackGain = feedback;
			this.DelayBuffer  = buffer;
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
		/// バッファサイズ
		/// </summary>
		public double DelayBuffer
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
			return new FractionalCombFilter(this.direct, this.effect, this.feedback, this.DelayBuffer);
		}
	}//class FractionalCombFilter
}
