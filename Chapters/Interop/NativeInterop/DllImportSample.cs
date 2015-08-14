using System;
using System.Runtime.InteropServices;

namespace NativeInterop
{
    class DllImportSample
    {
        public static void Main()
        {
            SYSTEMTIME t;
            GetLocalTime(out t);

            Console.WriteLine($"{t.wYear}/{t.wMonth}/{t.wDay} {t.wHour}:{t.wMinute}:{t.wSecond}");
        }

        [DllImport("kernel32.dll")]
        static extern void GetLocalTime(out SYSTEMTIME lpSystemTime);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}
