using System;

namespace SoundLibrary.Stream.Monaural
{
	/// <summary>
	/// 移調ストリーム。
	/// 別のストリームから読み出した音を移調して出力する。
	/// 音程と再生速度の両方が変わる。
	/// 線形補間によるアップサンプル/ダウンサンプルしているだけなので、
	/// 高音質を目指すなら、別途、アンチエイリアスフィルタをかける必要がある。
	/// </summary>
	public class RateTransposer : Stream
	{
		#region フィールド

		Stream stream; // inner stream
		double rate; // 変換レート。音程が rate 、再生時間が 1/rate 倍に。
		short[] inputBuffer; // 入力用バッファ。
		short prev; // 1音前のデータを一時的に保存しておく。

		#endregion
		#region 初期化

		/// <summary>
		/// 初期化。
		/// </summary>
		/// <param name="stream">内部ストリーム</param>
		/// <param name="rate">変換レート</param>
		public RateTransposer(Stream stream, double rate)
		{
			this.stream = stream;
			this.rate = rate;
			this.inputBuffer = null;
			this.prev = 0;
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// 変換レート。
		/// 音程が rate 、再生時間が 1/rate 倍に。
		/// </summary>
		public double Rate
		{
			get{return this.rate;}
			set{this.rate = value;}
		}

		#endregion
		#region Stream メンバ

		public override int FillBuffer(short[] buffer, int offset, int size)
		{
			// データを入力ストリームから読み出す。

			int inputSize = (int)(size * this.rate);

			if(this.inputBuffer == null || this.inputBuffer.Length < inputSize)
			{
				this.inputBuffer = new short[inputSize];
			}

			int readSize = this.stream.FillBuffer(this.inputBuffer, 0, inputSize);

			if(readSize != inputSize)
			{
				size = (int)(readSize / this.rate);
			}

			double delta = this.rate / 2;

			// サンプリングレートを変更しつつ出力バッファにデータをコピー。
			// Bresenham アルゴリズム的な動作。

			for(int i=0, j=offset;;)
			{
				while(delta >= 0)
				{
					buffer[j] = Interpolate(delta, this.prev, this.inputBuffer[i]); //! ←これ、prev と input[i] が逆。
					this.prev = this.inputBuffer[i]; //! ← これって delta < 0 の側にあるべきでは？

					delta -= this.rate;
					++j;
					if(j - offset >= size)
						goto END;
				}

				while(delta < 0)
				{
					delta += 1;
					++i;
					if(i >= inputSize)
						goto END;
				}
				//! ↑
				// while(!(j >= size + offset || i >= inputSize))
				//   if(delta >= 0) ....
				//   else ....
				// の方が自然なのでは？
			}
			END:
			return size;
		}

		public override bool Skip(int size)
		{
			int inputSize = (int)(size * this.rate);
			return this.stream.Skip(inputSize);
		}

		#endregion
		#region 補助関数(private)

		/// <summary>
		/// 線形補間関数。
		/// </summary>
		/// <param name="delta">val1 と val2 を混ぜる割合（整数部分は無視される）</param>
		/// <param name="val1">値1</param>
		/// <param name="val2">値2</param>
		/// <returns></returns>
		static short Interpolate(double delta, short val1, short val2)
		{
			delta -= (int)delta;
			double val = val1 + delta * (val2 - val1);
#if CHECK_RANGE
			if(val > short.MaxValue) val = short.MaxValue;
			if(val < short.MinValue) val = short.MinValue;
#endif
			return (short)val;
		}

		#endregion
	}
}
