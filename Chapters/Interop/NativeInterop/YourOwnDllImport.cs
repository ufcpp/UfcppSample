using System;
using System.Runtime.InteropServices;

namespace NativeInterop
{
    class YourOwnDllImport
    {
        [DllImport("Win32Dll.dll", CharSet = CharSet.Unicode)]
        extern static void FillA16(string s);

        [DllImport("Win32Dll.dll", CharSet = CharSet.Ansi)]
        extern static void FillA8(string s);

        [DllImport("Win32Dll.dll")]
        extern static int GetValue();

        public static void Main()
        {
            Console.WriteLine(GetValue());

            var s1 = "awsedrftgyhu";
            FillA8(s1);
            Console.WriteLine(s1);

            var s2 = "awsedrftgyhu";
            FillA16(s2);
            Console.WriteLine(s2);

            Console.ReadKey();
        }
    }
}
