namespace NativeInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            ComImportSample.Main();
            ComLateBindingSample.Main();
            WinRtSample.Main();
            DllImportSample.Main();
            YourOwnDllImport.Program.Main();
            Dll.StringSample.Main();
        }
    }
}
