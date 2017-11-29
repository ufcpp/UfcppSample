using System;

using SoundLibrary.Filter;

namespace SoundLibrary.Pipe.Monaural
{
	/// <summary>
	/// フィルタ処理を行うパイプ。
	/// </summary>
	public class FilteredPipe : Pipe
	{
		#region フィールド

		IFilter filter;

		#endregion
		#region 初期化

		public FilteredPipe(Queue input, Queue output, IFilter filter)
			: base(input, output)
		{
			this.filter = filter;
		}

		#endregion
		#region 処理

		public override void Process()
		{
			while(!this.input.IsEmpty)
			{
				double val = this.filter.GetValue(this.input.Front);
				this.output.Enqueue(SoundLibrary.Util.ClipShort(val));
				this.input.Dequeue();
			}
		}

		#endregion
	}
}
