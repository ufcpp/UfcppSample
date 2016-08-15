using System;

namespace Unsafe
{
    class Program
    {
        static void Main()
        {
            unsafe
            {
                int n;
                int* pn = &n;        // n のアドレスをポインター pn に代入。
                byte* p = (byte*)pn; // 違う型のポインターに無理やり代入可能。

                *p = 0x78; // n の最初の1バイト目に 0x78 を代入
                ++p;
                *p = 0x56; // n の2バイト目に 0x56 を代入
                ++p;
                *p = 0x34; // n の3バイト目に 0x34 を代入
                ++p;
                *p = 0x12; // n の4バイト目に 0x12 を代入

                Console.Write("{0:x}\n", n); // n の値を16進数で表示。
            }
        }
    }
}
