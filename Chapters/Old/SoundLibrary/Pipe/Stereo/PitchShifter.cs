using System;

using SoundLibrary.Filter.Equalizer;

namespace SoundLibrary.Pipe.Stereo
{
	/// <summary>
	/// ピッチシフトを行うクラス。
	/// 移調＋アンチエイリアス＋時間伸縮。
	/// </summary>
	public class PitchShifter : Pipe
	{
		#region フィールド

		TimeStretcher  ts; // 時間伸縮
		RateTransposer rt; // 
		FilteredPipe   fp;

		// rate >= 1 のとき、時間伸縮→アンチエイリアス→移調。
		// rate <  1 のとき、移調→アンチエイリアス→時間伸縮。
		CascadePipe cp;
		Pipe[] pipes;

		Queue temp1;
		Queue temp2;

		ParametricEqualizer fl, fr;
		Coefficient[] cd, cap; // fl, fr の係数と、そのアナログプロトタイプ。
		ParametricEqualizer.Parameter[] peq;

		#endregion
		#region 初期化

		public PitchShifter(Queue input, Queue temp1, Queue temp2, Queue output, int size, int overlap, double rate)
			: this(input, temp1, temp2, output, size, overlap, rate, TimeStretcher.DEFAULT_MAXSKIP)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public PitchShifter(Queue input, Queue temp1, Queue temp2, Queue output, int size, int overlap, double rate, int maxSkip)
			: base(input, output)
		{
			this.cap = new EllipticFilterDesigner(8, 0.99, 0.05).GetAnalogPrototype();
			this.cd= new Coefficient[this.cap.Length];
			for(int i=0; i<this.cd.Length; ++i) this.cd[i] = new Coefficient();
			this.peq = new ParametricEqualizer.Parameter[this.cap.Length];
			for(int i=0; i<this.peq.Length; ++i) this.peq[i] = new ParametricEqualizer.Parameter();
			this.pipes = new Pipe[3];

			this.fl = new ParametricEqualizer(this.peq);
			this.fr = new ParametricEqualizer(this.peq);

			this.ts = new TimeStretcher(input, output, size, overlap, 1.0/rate, maxSkip);
			this.rt = new RateTransposer(input, output, rate);
			this.fp = new FilteredPipe(input, output, fl, fr);
			this.temp1 = temp1;
			this.temp2 = temp2;

			this.SetRate(rate);
			this.cp = new CascadePipe(this.pipes);
		}

		public void SetRate(double rate)
		{
			if(rate >=1)
			{
				pipes[0] = this.ts; this.ts.InputQueue = this.input; this.ts.OutputQueue = this.temp1;
				pipes[1] = this.fp; this.fp.InputQueue = this.temp1; this.fp.OutputQueue = this.temp2;
				pipes[2] = this.rt; this.rt.InputQueue = this.temp2; this.rt.OutputQueue = this.output;
				FilterDesigner.BilinearTransform(this.cap, this.cd, Math.PI / rate);
				FilterDesigner.ToPeqCoefficient(this.cd, this.peq);
			}
			else
			{
				pipes[0] = this.rt; this.rt.InputQueue = this.input; this.rt.OutputQueue = this.temp1;
				pipes[1] = this.fp; this.fp.InputQueue = this.temp1; this.fp.OutputQueue = this.temp2;
				pipes[2] = this.ts; this.ts.InputQueue = this.temp2; this.ts.OutputQueue = this.output;
				FilterDesigner.BilinearTransform(this.cap, this.cd, Math.PI * rate);
				FilterDesigner.ToPeqCoefficient(this.cd, this.peq);
			}
			this.ts.SetRate(1.0/rate);
			this.rt.Rate = rate;

			this.fl.UpdateGain();
			this.fr.UpdateGain();
		}

		#endregion
		#region 処理

		public override void Process()
		{
			this.cp.Process();
		}

		#endregion
	}
}
