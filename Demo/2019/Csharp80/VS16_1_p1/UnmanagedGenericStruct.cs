namespace VS16_1_p1.UnmanagedGenericStruct
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void SafeStackalloc<T>()
            where T : unmanaged
        {
            Span<T> span = stackalloc T[4];
        }

        static void Main()
        {
            // 以前から OK
            SafeStackalloc<int>(); // 値型
            SafeStackalloc<DateTime>(); // 値型だけを含む構造体

#if false
            // 以下はNG
            SafeStackalloc<string>(); // 参照型
#endif

            // C# 8.0 で OK になった (ジェネリックな型、入れ子も OK)
            SafeStackalloc<KeyValuePair<int, int>>();
            SafeStackalloc<Wrap<int>>();
            SafeStackalloc<KeyValuePair<(float, bool), Wrap<int>>>();
        }

        struct Wrap<T>
            where T : unmanaged
        {
            public T Value;
        }
    }
}
