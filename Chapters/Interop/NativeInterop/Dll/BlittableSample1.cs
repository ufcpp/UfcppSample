namespace NativeInterop.Dll
{
    using System;
    using System.Runtime.InteropServices;

    enum Enum16 : ushort { }

    struct Struct16 { public byte x, y; }

    [StructLayout(LayoutKind.Explicit, Size = 2)]
    struct ExplicitStruct16 { }

    /// <summary>
    /// DllImport したメソッドの引数は、型のサイズしか見ていないという話。
    ///
    /// ネイティブ DLL 側で公開できる関数は extern "C" なものなので、シグネチャは関数名だけ(引数はシグネチャに含まれない。オーバーロード不可)。
    /// DllImport している C# 側としては、所定の順序で引数をスタックに積んで、指定された名前の関数を呼び出すだけ。
    /// なので、スタックへの引数の積まれ方が一緒なら呼べてしまう。
    ///
    /// もちろん、サイズが一致している保証が必要だし、ブロックコピーされて困る型は渡せないので、
    /// いわゆる blittable 型(プリミティブ型か他の blittable 型のみを含む型)でないとダメ。
    /// </summary>
    class BlittableSample1
    {
        // Native DLL 側には void SetValu(UINT16& x) が1つだけある。
        // 中で x に 0x1234 の値を代入してる。

        // ushort, Enum16, Struct16, ExplicitStruct16 はいずれも2バイトの値型。
        // 型のサイズさえ一致していれば DllImport で呼び出すことができる。

        [DllImport("Win32Dll.dll")]
        extern static void SetValue(ref ushort x);

        [DllImport("Win32Dll.dll")]
        extern static void SetValue(ref Enum16 x);
        [DllImport("Win32Dll.dll")]
        extern static void SetValue(ref Struct16 x);
        [DllImport("Win32Dll.dll")]
        extern static void SetValue(ref ExplicitStruct16 x);

        public static void Main()
        {
            // 以下の4つはどれも 1234 と表示される
            {
                ushort x = 0;
                SetValue(ref x);
                Console.WriteLine($"{x:X2}");
            }
            {
                // enum の underlying type が ushort なので ushort と互換
                var x = default(Enum16);
                SetValue(ref x);
                Console.WriteLine($"{(ushort)x:X2}");
            }
            {
                // byte 2つで合計2バイトな状態でも、ushort とサイズが一緒なので同じ関数を呼べる
                var x = default(Struct16);
                SetValue(ref x);
                Console.WriteLine($"{x.y:X2}{x.x:X2}");
            }
            {
                // ExplicitStruct16 にはメンバーがないけど、レイアウトを明示的に指定して2バイト化してる
                // その無理やり確保した2バイトの領域に 0x1234 が書き込まれる
                var x = default(ExplicitStruct16);
                SetValue(ref x);
                unsafe
                {
                    var p = (byte*)&x;
                    Console.WriteLine($"{p[1]:X2}{p[0]:X2}");
                }
            }
        }
    }
}
