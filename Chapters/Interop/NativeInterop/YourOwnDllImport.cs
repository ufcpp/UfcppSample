using System;
using System.Runtime.InteropServices;

namespace NativeInterop
{
    class YourOwnDllImport
    {
        [DllImport("Win32Dll.dll", CharSet = CharSet.Unicode)]
        extern static void FillA(string s);

        [DllImport("Win32Dll.dll")]
        extern static int GetValue();

        public static void Main()
        {
            Console.WriteLine(GetValue());
            var s = "awsedrftgyhu";
            FillA(s);
            Console.WriteLine(s);

            Console.ReadKey();
        }
    }
}
