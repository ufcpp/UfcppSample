using System;

namespace AppMain
{
	/// <summary>
	/// アプリケーションの設定。
	/// </summary>
	public class AppSettings
	{
		public int left;
		public int top;
		public int width;
		public int height;

		public WaveLoadFormSettings loadForm = new WaveLoadFormSettings();
	}
}
