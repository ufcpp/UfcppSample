#pragma warning disable 0219

namespace DigitSeparator
{
    class Program
    {
        static void Main()
        {
            // C# 7.0
            var b1 = 0b1111_0000;
            var x1 = 0x0001_F408;

            // C# 7.2
            // b, x の直後に _ 入れてもOKに
            var b2 = 0b_1111_0000;
            var x2 = 0x_0001_F408;

#if InvalidCode
            var a = _10;
            var b = 10_;
            var c = 1._0;
            var d = 1_.0;
#endif
        }
    }
}
