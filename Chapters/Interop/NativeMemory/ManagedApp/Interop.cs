using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static class Interop
    {
        public delegate void Callback(int senderId, IntPtr data, IntPtr dataLen);

        [DllImport("NativeLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCallback(Callback callback);

        [DllImport("NativeLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Trigger(int senderId);

        [DllImport("NativeLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void AddRef(IntPtr data);

        [DllImport("NativeLib.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Release(IntPtr data);
    }
}
