using System;

namespace SoundLibrary.Mathematics
{
	using Type = System.Double;

	/// <summary>
	/// Array の概要の説明です。
	/// </summary>
	public class Array
	{
		/// <summary>
		/// 配列を左右反転する。
		/// </summary>
		/// <param name="x">元の配列</param>
		/// <param name="y">結果格納先</param>
		public static Type[] Reverse(Type[] x)
		{
			int len = x.Length;
			Type[] y = new Type[len];
			for(int i=0, j=len-1; i<len; ++i, --j)
				y[j] = x[i];
			return y;
		}

		/// <summary>
		/// 配列 x を右に delay だけずらす。
		/// </summary>
		/// <param name="x">元の配列</param>
		/// <param name="y">結果格納先</param>
		public static void Delay(Type[] x, int delay, Type[] y)
		{
			int i = x.Length - 1;
			for(; i>=delay; --i)
				y[i] = x[i - delay];
			for(; i>=0; --i)
				y[i] = 0;
		}
	}
}
