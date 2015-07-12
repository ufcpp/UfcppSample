using System;

namespace SoundLibrary.Pipe.Monaural
{
	/// <summary>
	/// 移調を行うクラス。
	/// データの間を補完して、データ長を変えることで、音程を変更する。
	/// 再生時間も変わってしまう。
	/// </summary>
	/// <remarks>
	/// 線形補間によるアップサンプル/ダウンサンプルしているだけなので、
	/// 高音質を目指すなら、別途、アンチエイリアスフィルタをかける必要がある。
	/// </remarks>
	public class RateTransposer : Pipe
	{
		#region 定数

		const double DEFAULT_RATE = 1.0;

		#endregion
		#region フィールド

		// パラメータ
		double rate; // 変換レート。音程が rate 、再生時間が 1/rate 倍に。

		// 現在の状態
		short prev; // 1音前のデータを一時的に保存しておく。
		double delta; // Bresenham アルゴリズム（DDA）的な動作をするための状態変数。

		#endregion
		#region 初期化

		/// <summary>
		/// デフォルト値で初期化。
		/// </summary>
		/// <param name="input">入力キュー</param>
		/// <param name="output">出力キュー</param>
		public RateTransposer(Queue input, Queue output)
			: this(input, output, DEFAULT_RATE)
		{
		}

		/// <summary>
		/// パラメータの設定。
		/// </summary>
		/// <param name="input">入力キュー</param>
		/// <param name="output">出力キュー</param>
		/// <param name="rate">変換レート。音程が rate 倍、再生時間が 1/rate 倍に。</param>
		public RateTransposer(Queue input, Queue output, double rate)
			: base(input, output)
		{
			this.rate = rate;
			this.delta = rate / 2;
		}

		#endregion
		#region 処理

		public override void Process()
		{
			while(!this.input.IsEmpty)
			{
				if(this.delta >= 0)
				{
					short data = Interpolate(this.delta, this.input.Front, this.prev);
					this.output.Enqueue(data);

					delta -= this.rate;
				}
				else
				{
					this.prev = this.input.Front;
					this.input.Dequeue();

					delta += 1;
				}
			}
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
			return (short)val;
		}

		#endregion
	}
}
