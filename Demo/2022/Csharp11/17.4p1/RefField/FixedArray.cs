#pragma warning disable IDE0044, IDE0051, CS0169

using System.Runtime.InteropServices;

namespace RefField;

public ref struct FixedArray4<T>
{
    private T _x0, _x1, _x2, _x3;

#if NET7_PREVIEW7
    // ほんとは UnscopedRef 属性ってのがあって、それに対応した暁には解決するものの…
    // 現状、この行をコンパイルする手段がない。
    public ref T this[int index] => ref Unsafe.Add(ref _x0, index);
#endif

    // MemoryMarshal.CreateSpan は互換性のため / Unsafe な手段を取りたいときのために、
    // ref 引数が scoped ref になってる(.NET 7 から)。
    // なので、このコードはコンパイルできる。
    // ただし… これを TargetFramework net6.0 に変えたらコンパイルできなくなる(scoped 修飾子がないので)。
    public ref T this[int index] => ref MemoryMarshal.CreateSpan(ref _x0, 4)[index];
}
