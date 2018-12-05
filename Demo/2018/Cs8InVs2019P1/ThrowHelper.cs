namespace System
{
    internal class ThrowHelper
    {
        internal static void ThrowValueArgumentOutOfRange_NeedNonNegNumException() => throw new ArgumentOutOfRangeException("need non negative number");
    }
}