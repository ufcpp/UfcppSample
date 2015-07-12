using System;

namespace SoundLibrary.Filter.Delay
{
	/// <summary>
	/// 遅延器
	/// </summary>
	public class Delay : IDelay
	{
		CircularBuffer buf;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="taps">遅延タップ数</param>
		public Delay(int taps)
		{
			if(taps <= 0)
				this.buf = null;
			else
				this.buf = new CircularBuffer(taps);

			this.Clear();
		}

		/// <summary>
		/// 遅延タップ数
		/// </summary>
		public int Taps
		{
			get
			{
				if(this.buf == null) return 0;
				return this.buf.Length;
			}
			set
			{
				if(this.buf == null)
					this.buf = new CircularBuffer(value);
				else
					this.buf.Resize(value);
			}
		}

		#region IFilter メンバ

		/// <summary>
		/// フィルタリングを行う。
		/// </summary>
		/// <param name="x">フィルタ入力。</param>
		/// <returns>フィルタ出力</returns>
		public double GetValue(double x)
		{
			if(this.buf == null)
				return x;

			double tmp = this.buf.Top;
			this.buf.PushBack(x);
			return tmp;
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

		#endregion
		#region IClonable メンバ

		public object Clone()
		{
			return new Delay(this.buf.Length);
		}

		#endregion
		#region IDelay メンバ

		public double DelayTime
		{
			get
			{
				return this.Taps;
			}
			set
			{
				this.Taps = (int)value;
			}
		}

		public double GetValue()
		{
			if(this.buf == null)
				return 0;

			return this.buf.Top;
		}

		public void Push(double x)
		{
			this.buf.PushBack(x);
		}

		public double GetBufferValue(int n)
		{
			return this.buf[n-1];
		}

		#endregion
	}//class Delay
}
