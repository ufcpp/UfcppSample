using System.Runtime.CompilerServices;

namespace ConsoleApp1;

/// <summary>
/// 型情報 + ref。
/// </summary>
/// <remarks>
/// object だと値型の処理が悲惨なので ref で untyped にやり取りするための型を用意。
///
/// 中身、<see cref="TypedReference"/> とほぼ一緒。
/// ただ、そっちは ref field 実装前からあるせいで相当特別な制限かかってて使いづらい。
///
/// 名前一緒だと混乱しそうなのであえて省略形。
/// </remarks>
public readonly ref struct TypedRef(Type type, ref byte r)
{
    public readonly Type Type = type;
    public readonly UnsafeRef UnsafeReference = new(ref r);

    public static TypedRef Create<T>(ref T value)
    {
        return new TypedRef(typeof(T), ref Unsafe.As<T, byte>(ref value));
    }

    public ref T As<T>()
    {
        if (typeof(T) != Type) ThrowTypeNotMatched();
        return ref UnsafeReference.As<T>();
    }

    public void Set<T>(T value)
    {
        if (!typeof(T).IsAssignableTo(Type)) ThrowTypeNotMatched();
        UnsafeReference.As<T>() = value;
    }

    public T Get<T>()
    {
        if (!typeof(T).IsAssignableFrom(Type)) ThrowTypeNotMatched();
        return UnsafeReference.As<T>();
    }

    private void ThrowTypeNotMatched() => throw new InvalidCastException();
}
