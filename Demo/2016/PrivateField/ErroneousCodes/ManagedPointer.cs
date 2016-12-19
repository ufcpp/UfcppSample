using System.Runtime.InteropServices;

// 参照型を含む構造体
struct Wrapper { object _obj; }

class ManagedPointer
{
    public unsafe void X()
    {
        // Wrapper みたいに内部的に参照型のフィールドを持っている型は、本来はポインター化できない
        // sizeof 取得も本来はできない

        // unmanaged なメモリを確保
        // AllocHGlobal で取得したメモリ領域は初期化されている保証がない
        // 実行するたびに違う値が入ってる
        var p = Marshal.AllocHGlobal(sizeof(Wrapper));
        Wrapper a = *(Wrapper*)p;

        // ここで GC が発生したとすると、
        // GC が TaskAwaiter 中の Task のフィールド(未初期化)を参照する
        // 未初期化(= 意味のないランダムな値)な参照先を見に行こうとして死ぬ

        Marshal.FreeHGlobal(p);
    }
}
