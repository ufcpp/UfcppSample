namespace UtfString
{
    internal static class Constants
    {
        internal const byte InvalidCount = 0xff;
        internal static readonly CodePoint EoS = new CodePoint(0xffff_ffff);
        internal static readonly (CodePoint cp, byte count) End = (EoS, InvalidCount);
    }
}
