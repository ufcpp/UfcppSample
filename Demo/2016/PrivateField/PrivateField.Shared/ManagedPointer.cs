using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class ManagedPointer
{
    public unsafe void X()
    {
        // TaskAwaiter は内部的に Task クラスのフィールドを1個だけ持っている
        // 本来はポインター化できない
        var p = Marshal.AllocHGlobal(sizeof(TaskAwaiter));

        // PCL ではエラーにならない
        TaskAwaiter a = *(TaskAwaiter*)p;

        // ここで GC が発生したとするとまずい

        Marshal.FreeHGlobal(p);
    }
}
