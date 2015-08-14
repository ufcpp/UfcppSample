namespace NativeInterop
{
    class Program
    {
        static void Main(string[] args)
        {
            DllImportSample.Main();
            ComImportSample.Main();
            ComLateBindingSample.Main();
            WinRtSample.MainAsync().Wait();
        }
    }
}
