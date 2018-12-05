namespace Cs8InVs2019P1.InterpolatedVerbatimStrings
{
    class Program
    {
        static void Main()
        {
            var x = 1;

            // こっちは C# 6.0 からあるやつ。
            var s1 = $@"\\\ {x}";

            // これまでは $ と @ の順番逆にできなかった。
            // C# 8.0 から @$ でも OK。
            var s2 = @$"\\\ {x}";
        }
    }
}
