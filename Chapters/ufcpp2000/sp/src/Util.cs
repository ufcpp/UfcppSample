public static class Util
{
	/// <summary>
	/// x を超えない最大の2のべきを求める。
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	public static int FloorPower2(int x)
	{
		if (x == 0)
			return 0;

		int n = 1;
		for (; x != 1; x /= 2, n *= 2) ;
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
		if (x == 0) return 0;
		if (x == 1) return 1;

		return 2 * FloorPower2(x - 1);
	}
}
