using System.Runtime.CompilerServices;

namespace ConsoleApp1;

/// <summary>
/// ref。
/// </summary>
/// <remarks>
/// Unsafe.As&lt;T, byte&gt; で ref byte 化して参照を持つ。
/// ほぼ managed ポインター。
/// ref byte 直伝搬よりはマシ、程度。
/// </remarks>
public readonly ref struct UnsafeRef(ref byte r)
{
    public readonly ref byte UnsafeReference = ref r;

    public ref T As<T>()
    {
        return ref Unsafe.As<byte, T>(ref UnsafeReference);
    }

    public static UnsafeRef Create<T>(ref T value)
    {
        return new UnsafeRef(ref Unsafe.As<T, byte>(ref value));
    }
}
