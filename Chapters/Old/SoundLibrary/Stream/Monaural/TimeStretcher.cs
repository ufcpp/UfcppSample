using System;

namespace SoundLibrary.Stream.Monaural
{
	/// <summary>
	/// TimeStretcher の概要の説明です。
	/// </summary>
	public class TimeStretcher : Stream
	{
		#region フィールド

		Stream stream; // inner stream
		double rate; // 変換レート。再生時間が 1/rate 倍に。
		short[] inputBuffer; // 入力用バッファ。
		int readSize; // 既に読み出された状態にあるデータの長さ。
		short[] overlapBuffer; // オーバーラップ用バッファ。
		int margin; // オフセット探索用のマージン。

		const int DEFAULT_OVERLAP = 512;
		const int DEFAULT_MARGIN = 1024;

		#endregion
		#region 初期化

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="stream">内部ストリーム</param>
		/// <param name="rate">変換レート</param>
		public TimeStretcher(Stream stream, double rate) : this(stream, rate, DEFAULT_OVERLAP, DEFAULT_MARGIN)
		{
		}

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="stream">内部ストリーム</param>
		/// <param name="rate">変換レート</param>
		/// <param name="overlapSize">オーバーラップ部分の長さ</param>
		public TimeStretcher(Stream stream, double rate, int overlapSize, int margin)
		{
			this.stream = stream;
			this.rate = rate;
			this.inputBuffer = null;
			this.readSize = 0;
			this.overlapBuffer = new short[overlapSize];
			this.margin = margin;
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// 変換レート。
		/// 再生時間が 1/rate 倍に。
		/// </summary>
		public double Rate
		{
			get{return this.rate;}
			set{this.rate = value;}
		}

		/// <summary>
		/// 変換レート。
		/// 再生時間が temp 倍に。
		/// </summary>
		public double Tempo
		{
			get{return 1 / this.rate;}
			set{this.rate = 1 / value;}
		}

		/// <summary>
		/// 読み出しデータのマージン。
		/// </summary>
		public int Margin
		{
			get{return this.margin;}
			set{this.margin = value;}
		}

		#endregion
		#region Stream メンバ

		/// <remarks>
		/// size が overlapSize よりも小さいとき、動作保証対象外。
		/// </remarks>
		public override int FillBuffer(short[] buffer, int offset, int size)
		{
			// データを入力ストリームから読み出す。
			int overlap = this.overlapBuffer.Length;

			int frameSize = (int)(size * this.rate);
			int inputSize = size + overlap + this.margin;

			if(this.inputBuffer == null || this.inputBuffer.Length < inputSize)
			{
				this.inputBuffer = new short[inputSize];
			}

			if(inputSize > this.readSize)
			{
				int readSize = this.stream.FillBuffer(this.inputBuffer, this.readSize, inputSize - this.readSize);

				if(readSize + this.readSize != inputSize)
				{
					size = Math.Min(readSize + this.readSize, size);
					for(int i=readSize + this.readSize; i<inputSize; ++i)
					{
						this.inputBuffer[i] = 0;
					}
					inputSize = readSize + this.readSize;
				}
			}

			// フレーム開始オフセットの決定。
			int frameOffset = GetOffset(this.overlapBuffer, this.inputBuffer, this.margin);

			// オーバーラップ部分のコピー。
			Crossfade(this.overlapBuffer, this.inputBuffer, frameOffset, buffer);

			if(size > overlap)
			{
				// 非オーバーラップ部分のコピー。
				SoundLibrary.Wave.Util.MemCopy(this.inputBuffer, overlap + frameOffset, buffer, overlap, size - overlap);

				// 次のフレーム用のオーバーラップデータを一時バッファにコピー。
				SoundLibrary.Wave.Util.MemCopy(this.inputBuffer, size + frameOffset, this.overlapBuffer, 0, overlap);
			}

			// 次のフレームの準備。
			if(inputSize > frameSize)
			{
				// 過剰に読んだ分は残しておく。
				SoundLibrary.Wave.Util.MemCopy(this.inputBuffer, frameSize, this.inputBuffer, 0, inputSize - frameSize);
				this.readSize = inputSize - frameSize;
			}
			else
			{
				// 読み足りない分、空読みする。
				int skipSize = frameSize - inputSize;
				this.stream.Skip(skipSize);
				this.readSize = 0;
			}

			return size;
		}

		public override bool Skip(int size)
		{
			int inputSize = (int)(size * this.rate);
			return this.stream.Skip(inputSize);
		}

		#endregion
		#region 内部関数

		/// <summary>
		/// a と b の信号をクロスフェードさせながら混ぜる。
		/// </summary>
		/// <param name="a">信号 a</param>
		/// <param name="b">信号 b</param>
		/// <param name="offset">b のオフセット</param>
		/// <param name="dest">混ぜた信号の書き込み先</param>
		static void Crossfade(short[] a, short[] b, int offset, short[] dest)
		{
			int len = a.Length;

			for(int i=0; i<len; ++i)
			{
				int val = ((len - i) * a[i] + i * b[i + offset]) / len;
				dest[i] = (short)val;
			}
		}

		/// <summary>
		/// 2つの信号 a と b を混ぜるとき、最も違和感なく混ざる位置オフセットを探す。
		/// a と b の相互相関が最も高い位置を探す。
		/// </summary>
		/// <param name="a">信号 a</param>
		/// <param name="b">信号 b</param>
		/// <param name="max">探索範囲 [0, max)</param>
		/// <returns>位置オフセット</returns>
		static int GetOffset(short[] a, short[] b, int max)
		{
			//return 0;
			//*
			double maxVal = int.MinValue;
			int offset = 0;
			for(int i=0; i<max; ++i)
			{
				double x = Correlation(a, b, i);
				if(x > maxVal)
				{
					maxVal = x;
					offset = i;
				}
			}
			return offset;
			//*/
		}

		/// <summary>
		/// a と b+offset の相関値を求める。
		/// </summary>
		/// <param name="a">信号 a</param>
		/// <param name="b">信号 b</param>
		/// <param name="offset">b のオフセット</param>
		/// <returns></returns>
		static double Correlation(short[] a, short[] b, int offset)
		{
			double x = 0;
			double ax = 0;
			double bx = 0;
			for(int i=0; i<a.Length; ++i)
			{
				short ai = a[i];
				short bi = b[i + offset];
				x += ai * bi;
				ax += ai * ai;
				bx += bi * bi;
			}
			ax *= bx;
			if(ax == 0)
				return int.MaxValue;
			return x * x / ax;
		}

		#endregion
	}
}
