using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundLibrary.Misc
{
	/// <summary>
	/// 実行時間計測用のトレースクラス。
	///パフォーマンスカウンタを使って実行時間を評価。
	/// </summary>
	public class Trace : IDisposable
	{
		#region DLL Import

		[DllImport("kernel32.dll")]
		public extern static short QueryPerformanceCounter(ref long x);
		[DllImport("kernel32.dll")]
		public extern static short QueryPerformanceFrequency(ref long x);

		#endregion
		#region フィールド

		string message;
		long time;
		TextWriter writer;
		string format;

		#endregion
		#region 初期化

		public Trace() : this(string.Empty) {}
		public Trace(string message) : this(message, Console.Error, "{0}: {1}\n") {}

		public Trace(string message, TextWriter writer, string format)
		{
			this.message = message;
			this.writer  = writer;
			this.format  = format;

			this.time = 0;
			QueryPerformanceCounter(ref this.time);
		}

		#endregion
		#region IDisposable メンバ

		public void Dispose()
		{
			long t = 0;
			QueryPerformanceCounter(ref t);
			t = t - this.time;
			writer.Write(format, this.message, t);
		}

		#endregion
	}
}
