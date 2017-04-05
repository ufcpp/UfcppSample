using System;
using System.Runtime.InteropServices;

namespace UnsafeRefReturns
{
    namespace Pointer
    {
        /// <summary>
        /// ポインターに対しても同じ物が書ける。
        /// X で直接
        /// </summary>
        unsafe struct Point3D
        {
            int* _data;
            public Point3D(int* data) => _data = data;
            public ref int X => ref _data[0];
            public ref int Y => ref _data[1];
            public ref int Z => ref _data[2];
        }

        unsafe struct Triangle
        {
            public int* _data;
            public Triangle(int* data) => _data = data;
            public Point3D A => new Point3D(_data);
            public Point3D B => new Point3D(_data + 3);
            public Point3D C => new Point3D(_data + 6);
        }

        class Program
        {
            unsafe static void Main()
            {
                var nativePointer = Interop.Malloc(12);
                var native = new Point3D((int*)nativePointer);
                Interop.Free(nativePointer);

                var stackPointer = stackalloc int[4];
                var stack = new Point3D(stackPointer);

                int* data = stackalloc int[9];
                var t = new Triangle(data);
                t.A.X = 10;
            }
        }

        static class Interop
        {
            [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Malloc(int size);

            [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void Free(IntPtr ptr);
        }
    }
}
