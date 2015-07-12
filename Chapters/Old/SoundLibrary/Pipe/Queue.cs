using System;

namespace SoundLibrary.Pipe
{
	/// <summary>
	/// FIFO のバッファ。
	/// </summary>
	public class Queue
	{
		#region フィールド

		short[] buffer;
		int mask;
		int bottom;
		int top;

		#endregion
		#region 初期化

		/// <summary>
		/// 最大要素数 max のキューを作成。
		/// </summary>
		/// <param name="max">最大要素数</param>
		public Queue(int max)
		{
			max = SoundLibrary.BitOperation.CeilPower2(max);

			this.buffer = new short[max];
			this.mask = max - 1;
			this.bottom = 0;
			this.top = 0;
		}

		#endregion
		#region キューの操作

		/// <summary>
		/// キューが空かどうか。
		/// </summary>
		/// <returns>空のとき true</returns>
		public bool IsEmpty
		{
			get{ return this.bottom == this.top; }
		}

		/// <summary>
		/// キューの要素数を取得。
		/// </summary>
		public int Count
		{
			get
			{
				return this.mask & (this.bottom - this.top);
			}
		}

		/// <summary>
		/// キューに値を入れる。
		/// </summary>
		/// <param name="val">値</param>
		public void Enqueue(short val)
		{
			this.buffer[this.bottom] = val;
			this.bottom = this.mask & (this.bottom + 1);
		}

		/// <summary>
		/// キューの先頭の要素を取り出す。
		/// （値は出力しない。
		///   STL の queue のように、Front で取り出してから Dequeue するように。）
		/// </summary>
		public void Dequeue()
		{
			this.Dequeue(1);
		}

		/// <summary>
		/// キューから n 要素取り出す。
		/// </summary>
		public void Dequeue(int n)
		{
			this.top = this.mask & (this.top + n);
		}

		/// <summary>
		/// 先頭の要素を読み出す。
		/// </summary>
		public short Front
		{
			get{return this.buffer[this.top];}
		}

		/// <summary>
		/// 値の読み書き。
		/// </summary>
		public short this[int i]
		{
			get
			{
				i = this.mask & (this.top + i);
				return this.buffer[i];
			}
			set
			{
				i = this.mask & (this.top + i);
				this.buffer[i] = value;
			}
		}

		#endregion
	}
}
