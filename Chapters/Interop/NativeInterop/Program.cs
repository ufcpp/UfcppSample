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
            Dll.BlittableSample1.Main();
            Dll.BlittableSample2.Main();
            Dll.CallbackSample.Main();
        }
    }
}
