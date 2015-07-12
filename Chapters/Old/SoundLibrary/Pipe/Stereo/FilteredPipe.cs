using System;

using SoundLibrary.Filter;

namespace SoundLibrary.Pipe.Stereo
{
	/// <summary>
	/// フィルタ処理を行うパイプ。
	/// </summary>
	public class FilteredPipe : Pipe
	{
		#region フィールド

		IFilter filterL;
		IFilter filterR;

		#endregion
		#region 初期化

		public FilteredPipe(Queue input, Queue output, IFilter filter)
			: this(input, output, filter, (IFilter)filter.Clone())
		{
		}

		public FilteredPipe(Queue input, Queue output, IFilter filterL, IFilter filterR)
			: base(input, output)
		{
			this.filterL = filterL;
			this.filterR = filterR;
		}

		#endregion
		#region プロパティ

		/// <summary>
		/// L チャネルに掛けるフィルタ。
		/// </summary>
		public IFilter Left
		{
			get{return this.filterL;}
			set{this.filterL = value;}
		}

		/// <summary>
		/// R チャネルに掛けるフィルタ。
		/// </summary>
		public IFilter Right
		{
			get{return this.filterR;}
			set{this.filterR = value;}
		}

		#endregion
		#region 処理

		public override void Process()
		{
			while(this.input.Count >= 2)
			{
				double l = this.filterL.GetValue(this.input.Front);
				this.output.Enqueue(SoundLibrary.Util.ClipShort(l));
				this.input.Dequeue();

				double r = this.filterR.GetValue(this.input.Front);
				this.output.Enqueue(SoundLibrary.Util.ClipShort(r));
				this.input.Dequeue();
			}
		}

		#endregion
	}
}
