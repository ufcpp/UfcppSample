using System;

namespace SoundLibrary.Filter.Delay
{
	/// <summary>
	/// 分数遅延フィルタ。
	/// </summary>
	public class FractionalDelay : IDelay
	{
		CircularBuffer buf;
		int integer;
		double fraction;
		double[] coef; // 分数遅延をかけるための FIR 係数

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="time">遅延タップ数</param>
		public FractionalDelay(double time) : this(time, 4){}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="time">遅延タップ数</param>
		/// <param name="firLength">分数遅延FIRの次数</param>
		public FractionalDelay(double time, int firLength)
		{
			if(time < 0)
			{
				this.integer = 0;
				this.fraction = 0;
				this.buf = null;
				this.coef = null;
			}
			else
			{
				this.coef = new double[firLength];
				GetFractionalDelayCoef(this.Delay, this.Length, this.coef);
				this.DelayTime = time;
			}

			this.Clear();
		}


		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			if(this.buf == null)
				return x;

			int n = this.integer - this.Delay;
			int i = 0;
			if(n < 0)
			{
				i = -n;
				n = 0;
			}

			this.buf.PushFront(x);

			double y = 0;
			for(; i<this.Length; ++i, ++n)
				y += this.buf[n] * this.coef[i];
			return y;
		}

		/// <summary>
		/// 値を循環バッファにプッシュする。
		/// </summary>
		/// <param name="x">プッシュする値</param>
		public void Push(double x)
		{
			if(this.buf == null)
				return;

			this.buf.PushFront(x);
		}

		/// <summary>
		/// 値の取り出し。
		/// </summary>
		/// <returns>フィルタ結果</returns>
		public double GetValue()
		{
			if(this.buf == null)
				return 0;

			int n = this.integer - this.Delay;
			int i = 0;
			if(n < 0)
			{
				i = -n;
				n = 0;
			}

			double y = 0;
			for(; i<this.Length; ++i, ++n)
				y += this.buf[n] * this.coef[i];
			return y;
		}

		public double GetBufferValue(int n)
		{
			if(n > this.Length)
				return this.buf[n-1];
			else
				return 0.0;
		}

		/// <summary>
		/// 内部状態のクリア
		/// </summary>
		public void Clear()
		{
			if(this.buf == null)
				return;

			for(int i=0; i<this.buf.Length; ++i)
			{
				this.buf[i] = 0;
			}
		}

		/// <summary>
		/// 遅延タップ数
		/// </summary>
		public double DelayTime
		{
			get
			{
				return this.Integer + this.Fraction;
			}
			set
			{
				this.Integer = (int)value;
				this.Fraction = value - integer;
			}
		}

		/// <summary>
		/// 遅延タップ数の整数部分
		/// </summary>
		public int Integer
		{
			get
			{
				return this.integer;
			}
			set
			{
				this.integer = value;
				if(this.buf == null)
					this.buf = new CircularBuffer(this.BufferSize);
				else if(this.buf.Length < this.BufferSize)
					this.buf.Resize(this.BufferSize);
			}
		}

		/// <summary>
		/// 遅延タップ数の小数部分
		/// </summary>
		public double Fraction
		{
			get
			{
				return this.fraction;
			}
			set
			{
				if(this.coef == null)
				{
					this.fraction = value;
					this.coef = GetFractionalDelayCoef(this.fraction + this.Delay, this.Length);
				}
				if(this.fraction != value)
				{
					this.fraction = value;
					GetFractionalDelayCoef(this.fraction + this.Delay, this.Length, this.coef);
				}
			}
		}

		/// <summary>
		/// 分数遅延FIRのタップ数。
		/// </summary>
		public int Length
		{
			get{return this.coef.Length;}
			set
			{
				if(this.coef.Length < value)
				{
					this.coef = GetFractionalDelayCoef(this.fraction + this.Delay, this.Length);
				}

				if(this.buf.Length < this.BufferSize)
					this.buf.Resize(this.BufferSize);
			}
		}

		int Delay{get{return this.Length / 2 - 1;}}
		int BufferSize{get{return this.integer + this.Length - this.Delay;}}

		public object Clone()
		{
			return new FractionalDelay(this.DelayTime);
		}

		#region static メソッド

		/// <summary>
		/// ディレイ値から分数遅延 FIR フィルタ係数を計算する。
		/// </summary>
		/// <param name="delay">ディレイ値</param>
		/// <param name="length">FIR フィルタのタップ数</param>
		/// <returns>FIR フィルタ係数</returns>
		static double[] GetFractionalDelayCoef(double delay, int length)
		{
			double[] coef = new double[length];
			GetFractionalDelayCoef(delay, length, coef);
			return coef;
		}

		static void GetFractionalDelayCoef(double delay, int length, double[] coef)
		{
			//*
			SpectrumAnalysis.Spectrum tmp = SpectrumAnalysis.Spectrum.FromDelay(delay, length);
			tmp.GetTimeSequence(coef);
			//*/

			/*
			for(int i=0; i<length; ++i)
			{
				coef[i] = 1;
				for(int j=0; j<length; ++j)
				{
					if(i == j) continue;

					coef[i] *= (delay - j) / (i - j);
				}
			}
			//*/
		}

		#endregion
	}
}
