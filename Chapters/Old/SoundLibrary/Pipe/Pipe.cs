using System;

namespace SoundLibrary.Pipe
{
	/// <summary>
	/// キューの間に入って処理を行うパイプ。
	/// </summary>
	public abstract class Pipe
	{
		#region フィールド

		protected Queue input;
		protected Queue output;

		#endregion
		#region 初期化

		public Pipe(Queue input, Queue output)
		{
			this.input = input;
			this.output = output;
		}

		#endregion
		#region 処理

		/// <summary>
		/// input キューと output キューの間に挟む処理。
		/// デフォルトは素通し。
		/// </summary>
		public virtual void Process()
		{
			while(!this.input.IsEmpty)
			{
				this.output.Enqueue(input.Front);
				this.input.Dequeue();
			}
		}

		/// <summary>
		/// キューの間に処理なしで、とにかく input 内のデータをフラッシュする。
		/// </summary>
		public virtual void Flush()
		{
			while(!this.input.IsEmpty)
			{
				this.output.Enqueue(input.Front);
				this.input.Dequeue();
			}
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// 入力キューの取得。
		/// </summary>
		public Queue InputQueue
		{
			set{this.input = value;}
			get{return this.input;}
		}

		/// <summary>
		/// 出力キューの取得。
		/// </summary>
		public Queue OutputQueue
		{
			set{this.output = value;}
			get{return this.output;}
		}

		#endregion
	}
}
