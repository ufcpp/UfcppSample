using System.Runtime.InteropServices;

namespace NativeInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            YourOwnDllImport.Main();

            //DllImportSample.Main();
            //ComImportSample.Main();
            //ComLateBindingSample.Main();
            //WinRtSample.Main();
        }
    }
}
