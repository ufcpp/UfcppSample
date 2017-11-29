using System;

namespace SoundLibrary
{
	/// <summary>
	/// ビット演算用クラス。
	/// </summary>
	public class BitOperation
	{
		/// <summary>
		/// Floor(Log_2(x)) を求める。
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int FloorLog2(int x)
		{
			if(x == 0)
				return 0;

			int n = 0;
			for(; x!=1; x/=2, ++n);
			return n;
		}

		/// <summary>
		/// Ceil(Log_2(x)) を求める。
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int CeilLog2(int x)
		{
			if(x == 0)
				return 0;

			return 1 + FloorLog2(x - 1);
		}

		/// <summary>
		/// x を超えない最大の2のべきを求める。
		/// 2^FloorLog2(x)
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		[System.Obsolete("FloorPower2 に移行")]
		public static int Power2(int x)
		{
			return FloorPower2(x);
		}

		/// <summary>
		/// x を超えない最大の2のべきを求める。
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int FloorPower2(int x)
		{
			if(x == 0)
				return 0;

			int n = 1;
			for(; x!=1; x/=2, n*=2);
			return n;
		}

		/// <summary>
		/// x 以上の最小の2のべきを求める。
		/// 2^CeilLog2(x)
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static int CeilPower2(int x)
		{
			if(x == 0)
				return 0;

			return 2 * FloorPower2(x - 1);
		}

		/// <summary>
		/// 下位 n ビットが1、残りが0のマスクを作る。
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int Mask(int n)
		{
			return (1 << n) - 1;
		}

		/// <summary>
		/// 四捨五入しつつシフト。
		/// </summary>
		/// <param name="val">値</param>
		/// <param name="shift">シフト量</param>
		/// <returns>シフト後の値</returns>
		public static int RoundShift(long val, int shift)
		{
			return (int)((val + (1 << (shift - 1))) >> shift);
		}
	}
}
