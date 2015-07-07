using static System.Math;

public static class NumericExtensions
{
    public static int Clip(this int x, int min, int max)
        => Max(min, Min(x, max));
}
