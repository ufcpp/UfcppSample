using System.Runtime.CompilerServices;

namespace RefField;

// やってることは System.TypedReference とほぼ一緒。
// TypedReference の利用条件緩和が掛かればそのまま使えるかも。
readonly ref struct TypedRef
{
    public readonly Type Type;
    private readonly ref byte _reference;

    private TypedRef(Type type, ref byte reference)
    {
        Type = type;
        _reference = ref reference;
    }

    public static TypedRef Create<T>(ref T reference) => new(typeof(T), ref Unsafe.As<T, byte>(ref reference));

    public ref T Cast<T>()
    {
        if (typeof(T) != Type) throw new InvalidCastException();
        return ref Unsafe.As<byte, T>(ref _reference);
    }
}

// TypedRef 利用例。
struct A
{
    public int X;
    public float Y;
    public string Z;

    public override string ToString() => $"{X}, {Y}, {Z}";

    // リフレクションは重たいし、↓こんなコードをコード生成するとして…
    public object GetValue(int index) => index switch
    {
        0 => X, // box 化が気になる。
        1 => Y, // box 化が気になる。
        2 => Z,
        _ => throw new IndexOutOfRangeException(),
    };

    public void SetValue(int index, object value)
    {
        switch (index)
        {
            case 0: X = (int)value; break; // box 化が気になる。
            case 1: Y = (float)value; break; // box 化が気になる。
            case 2: Z = (string)value; break;
            default: throw new IndexOutOfRangeException();
        }
    }
}

static class AExtensions
{
    // 代わりにこんなの活用できないかなぁとか思う。
    // (UnscopedRef 属性がサポートされたらインスタンス メソッドにできる。
    //  .NET 7 Preview 7 時点ではまだなので、いったん拡張メソッド。)
    public static TypedRef GetReference(ref this A a, int index) => index switch
    {
        0 => TypedRef.Create(ref a.X),
        1 => TypedRef.Create(ref a.Y),
        2 => TypedRef.Create(ref a.Z),
        _ => throw new IndexOutOfRangeException(),
    };
}

partial class Program
{
    public static void TypedRefExample()
    {
        var a = new A();

        a.SetValue(0, 1);    // box 化
        a.SetValue(1, 1.2f); // box 化
        a.SetValue(2, "abc");

        Console.WriteLine(a);

        // 速くなるかどうかはちょっと怪しい(測ってみないとわかんない)けど、一応 box 化は避けれる。
        a.GetReference(0).Cast<int>() = 2;
        a.GetReference(1).Cast<float>() = 2.5f;
        a.GetReference(2).Cast<string>() = "xyz";

        Console.WriteLine(a);
    }
}
