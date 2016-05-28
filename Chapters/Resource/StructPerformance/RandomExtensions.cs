using System;

public static class RandomExtensions
{
    /// <summary>
    /// min ～ max の範囲の乱数。
    /// </summary>
    /// <param name="r"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double NextDouble(this Random r, double min, double max) => r.NextDouble() * (max - min) + min;
}
