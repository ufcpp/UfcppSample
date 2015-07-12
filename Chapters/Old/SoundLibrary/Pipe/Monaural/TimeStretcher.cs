#define A

using System;

namespace SoundLibrary.Pipe.Monaural
{
	/// <summary>
	/// 時間伸長・短縮処理を行うクラス。
	/// 一定間隔でデータを間引き・コピーすることで、
	/// 音程を変えることなく再生時間を伸長・短縮する。
	/// </summary>
	public class TimeStretcher : Pipe
	{
		#region 定数

		const int DEFAULT_SIZE = 2048;
		const int DEFAULT_OVERLAP = 256;
		const int DEFAULT_MAXSKIP = 512;
		const double DEFAULT_RATE = 1.0;

		#endregion
		#region フィールド

		#region 内部クラス（オーバーラップ用の一時バッファ）

		struct OverlapBuffer
		{
			short[] buffer;
			int write; // 書き込み位置
			int read;  // 読み込み位置

			public OverlapBuffer(int size, int write)
			{
				this.buffer = new short[size];
				this.write = write;
				this.read = 0;
			}

			/// <summary>
			/// キューのサイズ設定。
			/// </summary>
			/// <param name="size">サイズ</param>
			/// <param name="full">バッファの初期状態（true: full, false: empty で開始）</param>
			public void SetSize(int size, int write)
			{
				this.buffer = new short[size];
				this.write = write;
				this.read = 0;
			}

			/// <summary>
			/// 要素数を取得。
			/// </summary>
			public int Count
			{
				get{return this.buffer.Length;}
			}

			public short this[int i]
			{
				get{return this.buffer[i];}
			}

			/// <summary>
			/// 値を挿入。
			/// </summary>
			/// <param name="data">値</param>
			public void Enqueue(short data)
			{
				this.buffer[this.write] = data;
				++this.write;
			}

			/// <summary>
			/// 値を取り出し。
			/// </summary>
			/// <returns>値</returns>
			public short Dequeue()
			{
				short data = this.buffer[this.read];
				++this.read;

				if(this.read == this.buffer.Length)
				{
					this.write = this.read = 0;
				}

				return data;
			}

			/// <summary>
			/// 書き込みバッファが残っているかどうか。
			/// </summary>
			/// <returns>フルなら true</returns>
			public bool Full()
			{
				return this.write == this.buffer.Length;
			}

			/// <summary>
			/// 読み出しデータが残っているかどうか。
			/// </summary>
			/// <returns>空っぽなら true</returns>
			public bool Empty()
			{
				return this.write == this.read;
			}
		}

		#endregion

		// パラメータ
		int size;
		int overlapSize;
		int frameSize;
		int maxSkip;

		int skip;
/*
・概要
    フレーム 1                       フレーム 2
  (1)    (2)    (3)                (1)    (2)    (3)
|----|--------|----|-- ....... --|----|--------|----|-- ....

フレーム k の(3)とフレーム k+1 の(1) をクロスフェードさせて、2つのフレームを繋ぐ。
ただし、オーバーラップ時に音が変にならないように、

k(3) と K+1(1) の相関値が高くなるようにフレームの先頭何サンプルかを読み飛ばす。
（相関値によって決めた読み飛ばしサンプル数は変数 skip に保存しておく。）

便宜上、
(1) を overlap(前)
(2) を non overlap
(3) を overlap(後)
と呼ぶ。

・上記のパラメータの意味
  (1)    (2)    (3)                (1)    (2)    (3)
|----|--------|----|-- ....... --|----|--------|----|
<------------------------------->
<------------>      ↑
<--->   ↑       frameSize
  ↑   size
overlapSize

frameSize = rate * size
長さ frameSize のデータが長さ size に伸縮する。
→ 再生時間が 1/rate 倍に。
 */

		// 現在の状態
		int current;
		// オーバーラップ用の一時バッファ
		OverlapBuffer buffer;

		#endregion
		#region 初期化

		/// <summary>
		/// デフォルト値で初期化。
		/// </summary>
		/// <param name="input">入力キュー</param>
		/// <param name="output">出力キュー</param>
		public TimeStretcher(Queue input, Queue output)
			: this(input, output, DEFAULT_SIZE, DEFAULT_OVERLAP, DEFAULT_RATE)
		{
		}

		/// <summary>
		/// パラメータの設定。
		/// </summary>
		/// <param name="input">入力キュー</param>
		/// <param name="output">出力キュー</param>
		/// <param name="size">ブロックサイズ</param>
		/// <param name="overlap">オーバーラップさせる部分の長さ</param>
		/// <param name="rate">変換レート。再生時間が 1/rate 倍に。</param>
		public TimeStretcher(Queue input, Queue output, int size, int overlap, double rate)
			: this(input, output, size, overlap, rate, DEFAULT_MAXSKIP)
		{
		}

		public TimeStretcher(Queue input, Queue output, int size, int overlap, double rate, int maxSkip)
			: base(input, output)
		{
			this.SetParameter(size, overlap, rate, maxSkip);
		}

		/// <summary>
		/// パラメータの設定。
		/// </summary>
		/// <param name="size">ブロックサイズ</param>
		/// <param name="overlap">オーバーラップさせる部分の長さ</param>
		/// <param name="rate">変換レート。再生時間が 1/rate 倍に。</param>
		public void SetParameter(int size, int overlap, double rate, int maxSkip)
		{
			this.size = size;
			this.overlapSize = overlap;
			this.frameSize = (int)(size * rate);
			this.maxSkip = maxSkip;

			this.last = this.size < this.frameSize ?
				this.frameSize + this.maxSkip :
				this.size + this.overlapSize + this.maxSkip;

			this.buffer = new OverlapBuffer(overlap, overlap);
			this.current = 0;
			this.skip = 0;
		}

		#endregion
		#region 処理

		int maxCorrelation;
		int skipNext;
		int last;

		int Correlation(int pos, int len)
		{
			long corr = 0;
			for(int i=0, j=pos; i<len; ++i, ++j)
			{
				short x = this.buffer[i];
				short y = this.input[j];

				corr += x * y;
			}
			return SoundLibrary.BitOperation.RoundShift(corr, 16);
#if false
			long corr = 0;
			long xabs = 0; long yabs = 0;

// 厳密に相関値を計算（計算量大）。
// Corr = E[xy]^2 / E[x^2]E[y^2]
// ここまでする必要はなさげ。

			for(int i=0, j=pos; i<len; ++i, ++j)
			{
				short x = this.buffer[i];
				short y = this.input[j];

				corr += x * y;
				xabs += x * x;
				yabs += y * y;
			}
			if(xabs == 0 || yabs == 0)
			{
				corr = int.MaxValue;
			}
			else
			{
				float temp = xabs;
				temp *= yabs;
				temp = (float)Math.Sqrt(temp);
				temp = corr / temp;
				corr = (long)(int.MaxValue * temp);
			}
			return (int)corr;
#endif
		}

		/// <summary>
		/// 現在位置(current)を更新する。
		/// ついでに、ピッチ予測とかの処理も行う。
		/// </summary>
		void MoveNext()
		{
			int len = this.buffer.Count;

			int start, offset;
			if(this.size < this.frameSize)
			{
				start = this.frameSize;
				offset = 0;
			}
			else
			{
				start = this.size + this.overlapSize;
				offset = this.frameSize - this.size;
			}

			if(this.current >= start &&
				this.current < start + this.maxSkip)
			{
				int corr = this.Correlation(this.current +offset, len);

				if(this.maxCorrelation < corr)
				{
					this.maxCorrelation = corr;
					this.skipNext = this.current + offset;
				}
			}

			++this.current;
		}

		/// <summary>
		/// 現在位置をリセットする(current を 0 に)。
		/// ついでに、スキップ量を更新。
		/// </summary>
		void Reset()
		{
			this.skip = this.skipNext == 0 ? 0 : this.skipNext - this.frameSize;

			this.maxCorrelation = int.MinValue;
			this.skipNext = 0;
			this.current = 0;
		}

		public override void Process()
		{
			// overlap(前)
			while(
				this.current < this.overlapSize &&
				this.input.Count > this.current + this.overlapSize + this.skip)
			{
				System.Diagnostics.Debug.Assert(!this.buffer.Empty());

				short temp1 = this.buffer.Dequeue();
				short temp2 = this.input[this.current + this.skip];
				short data = Interpolate(temp1, temp2, this.current, this.overlapSize);
				this.output.Enqueue(data);

				this.MoveNext();
			}

			// non overlap
			while(
				this.current >= this.overlapSize &&
				this.current < this.size &&
				this.input.Count > this.current + this.overlapSize + this.skip)
			{
				short data = this.input[this.current + this.skip];
				this.output.Enqueue(data);

				this.MoveNext();
			}

			// overlap(後)
			while(
				this.current >= this.size &&
				this.current < this.size + this.overlapSize &&
				this.input.Count > this.current + this.overlapSize + this.skip)
			{
				short data = this.input[this.current + this.skip];
				this.buffer.Enqueue(data);

				this.MoveNext();
			}

			// 次フレームの開始地点まで読み飛ばし。
			while(
				this.current >= this.size + this.overlapSize &&
				this.current < this.last &&
				this.input.Count > this.current + this.overlapSize + this.skip)
			{
				this.MoveNext();
			}

			// 次のフレームに移行
			if(this.current == this.last)
			{
				if(this.input.Count >= this.frameSize)
				{
					this.input.Dequeue(this.frameSize);
					this.Reset();
				}
			}
		}

		/// <summary>
		/// フレームのスキップ数を求める。
		/// 前フレームの overlap(後) と現フレームの overlap(前) の相互相関値が高くなるように、
		/// フレームの最初数サンプルをスキップする。
		/// </summary>
		/// <remarks>
		/// スキップ数を決定するために、何サンプルか後ろのデータを参照するため、
		/// 入力キューにある程度データがたまらないとスキップ数を決定できない。
		/// スキップ数を決定できない間は false を返す。
		/// </remarks>
		/// <returns>スキップ数を決定できた場合 true を返す</returns>
		bool SeekSkipSize()
		{
			if(this.input.Count < this.maxSkip + this.buffer.Count)
				return false;

			long corrMax = long.MinValue;
			this.skip = 0;

			for(int offset = 0; offset<this.maxSkip; ++offset)
			{
				long corr = 0;
				for(int i=0; i<this.buffer.Count; ++i)
					corr += this.input[i + offset] * this.buffer[i];

				if(corr > corrMax)
				{
					corrMax = corr;
					this.skip = offset;
				}
			}

			return true;
		}

		#endregion
		#region 内部関数

		/// <summary>
		/// a と b の信号をクロスフェードさせながら混ぜる。
		/// </summary>
		/// <param name="a">信号 a</param>
		/// <param name="b">信号 b</param>
		/// <param name="fade">混ぜる比率</param>
		/// <param name="overlap">混ぜる区間の長さ</param>
		static short Interpolate(short a, short b, int fade, int overlap)
		{
			int val = (overlap - fade) * a + fade * b;
			val /= overlap;
			return (short)val;
		}

		#endregion
	}
}
