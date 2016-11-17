using System;
using System.Runtime.CompilerServices;

namespace SampleApp.Lib
{
    public struct TypedPointer
    {
        public IntPtr Pointer;
        public Type Type;
        public TypedPointer(IntPtr pointer, Type type)
        {
            Pointer = pointer;
            Type = type;
        }

        public static unsafe TypedPointer Create<T>(ref T value) => new TypedPointer((IntPtr)Unsafe.AsPointer(ref value), typeof(T));

        public ref T Ref<T>() => ref Ref<T>(Pointer, Type);

        private static unsafe ref T Ref<T>(IntPtr pointer, Type type) => ref Unsafe.AsRef<T>((void*)pointer); //todo: 型チェック
    }
}
