namespace NativeInterop.YourOwnDllImport
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 自前で作った Win32 DLL を DllImport する例。
    ///
    /// とりあえず一番シンプルな例。
    /// ネイティブ側から値を1個もらうだけ。
    /// </summary>
    class Program
    {
        [DllImport("Win32Dll.dll")]
        extern static int GetValue();

        private static IntPtr GetAddress(object x)
        {
            return System.Runtime.CompilerServices.Unsafe.As<object, IntPtr>(ref x);
        }

        [StructLayout(LayoutKind.Sequential)]
        class CallbackParam
        {
            public uint Value;
        }

        public static void Main()
        {
            Console.WriteLine(GetValue());

#if false

            // GC 誘発
            for (int i = 0; i < 10000; i++) { var x = new byte[10000]; }

            var param = new CallbackParam();
            var h = GCHandle.Alloc(param, GCHandleType.Pinned);

            Sub(h);

            //Console.WriteLine($"&d = {GetAddress(param)}, &c = {GetAddress(c)}, fp(c) = {Marshal.GetFunctionPointerForDelegate(c)}");

            // GC 誘発
            for (int i = 0; i < 10000; i++) { var x = new byte[10000]; }
            GC.Collect(2, GCCollectionMode.Forced);

            //Console.WriteLine($"&d = {GetAddress(param)}, &c = {GetAddress(c)}, fp(c) = {Marshal.GetFunctionPointerForDelegate(c)}");

            for (uint i = 0; i < 5; i++)
            {
                FireCallback(i);
            }

            //Console.WriteLine($"&d = {GetAddress(param)}, &c = {GetAddress(c)}, fp(c) = {Marshal.GetFunctionPointerForDelegate(c)}");

            return;


        private static void Sub(GCHandle h)
        {
            Callback c = (param, value) =>
            {
                var obj = (CallbackParam)GCHandle.FromIntPtr(param).Target;
                obj.Value += value;
                Console.WriteLine($"{obj.Value} {value}");
            };

            SetCallback(GCHandle.ToIntPtr(h), c);
        }
#endif
        }
    }
}
