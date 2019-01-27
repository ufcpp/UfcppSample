using System;
using System.Runtime.InteropServices;

namespace Preview2.BoolExhaustiveness
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(BoolToInt(false)); // 0
            Console.WriteLine(BoolToInt(true)); // 1
            Console.WriteLine(BoolToInt(GetBool(2))); // C# 7.3 までは -1、C# 8.0 では 1
        }

        static int BoolToInt(bool b)
        {
            // C# 8.0 から、この switch が if (b) return 1; else return 0; に最適化されるようになった。
            // 要は、「0 でなければ true」という扱い。
            //
            // その代わり、case に false/true が並んでれば「全パターンを網羅」判定を受けるようになった。
            // (「確実な代入」ルールとか、switch 式の網羅性チェックで便利。)
            switch (b)
            {
                case false: return 0;
                case true: return 1;

                // unsafe コードとかを使えば false でも true でもない値を作れる。
                // C# 7.3 までは、そういう「false でも true でもない値」を渡すとここに来てた。
                // C# 8.0 では case true の方に行くようになったので、このコードは「到達不能なコードがある」警告が出るようになった。
                default: return -1;
            }
        }

        static bool GetBool(byte b)
        {
            var u = new Union();
            u.Byte = b;
            return u.Boolean;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct Union
        {
            [FieldOffset(0)]
            public byte Byte;
            [FieldOffset(0)]
            public bool Boolean;
        }
    }
}
