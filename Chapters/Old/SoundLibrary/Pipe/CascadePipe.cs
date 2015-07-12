using System;

namespace SoundLibrary.Pipe
{
	/// <summary>
	/// Pipe を直列に繋ぐ。
	/// </summary>
	public class CascadePipe : Pipe
	{
		#region フィールド

		Pipe[] pipes;

		#endregion
		#region 初期化

		public CascadePipe(params Pipe[] pipes)
			: base(pipes[0].InputQueue, pipes[pipes.Length - 1].OutputQueue)
		{
			this.pipes = pipes;
		}

		#endregion
		#region 処理

		public override void Process()
		{
			foreach(Pipe pipe in this.pipes)
				pipe.Process();
		}

		#endregion
	}
}
