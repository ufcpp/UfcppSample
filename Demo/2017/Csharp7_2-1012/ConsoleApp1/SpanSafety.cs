//#define InvalidCode

// System.Span<T> 構造体は System.Memory パッケージ内に定義されてる

using System;

namespace SpanSafety
{
    class Program
    {
        static void Main()
        {
            byte x = 0;
            ref var y = ref ValidRef(ref x);

            var a = new byte[] { 1, 2, 3 };
            var b = ValidSpan(a);

            // その他、ref struct (Span<T> など) は、box 化してはいけないという制約があって、
            // その結果、
            // - ToString とか呼べない
            // - ラムダ式にキャプチャできない
            // - async/await と併用できない
            // などの制限あり
            //
            // ちなみにその理由は、
            // - ref は GC によるオブジェクトの移動に追従する必要があるけど、その追跡がスタック上でしかできない
            // - Span<T> は、ref T とサイズのペアだけど、Span<T> 自身の書き換えはアトミックじゃない
            //    - ref T だけ書き換わって、サイズが書き換わってない不整合があるとセキュリティホール(buffer over run)になる
            //    - スタック上に限定してしまえば、不正な状態で読み書きされることはなくなる
            // - スタック上の領域を参照している Span<T> を、ヒープ上に持っていかれると困る
            //    - スタック上の領域はすぐに消える。消えた場所を参照するわけにはいかない

#if InvalidCode
            Span<byte> s1 = stackalloc byte[10];

            var str = s1.ToString();

            Func<byte> a1 => () => s1[0];

            async Task X()
            {
                Span<byte> s2 = stackalloc byte[10];
                await Task.Delay(1);
                s2[0] = 1;
            }

            // 余談: TypedReference 型も、今思えば、分類としては ref struct になるんだけど
            // C# 7.1 まで、仕様漏れでまずい状態だったらしい
            TypedReference tr = __makeref(x);
            Func<int> a2 = tr.GetHashCode; // C# 7.1 まではコンパイルできてた。本来、まずい。要はコンパイラーの仕様漏れ
#endif
        }

        #region C# 7.0 で参照戻り値追加

        // 引数由来の参照を戻り値に返すのはOK
        static ref byte ValidRef(ref byte x) => ref x;

#if InvalidCode
        // ローカル由来の参照を戻り値に返すのはNG
        static ref byte InvalidRef()
        {
            byte x = 1;
            return ref x;
        }
#endif

#endregion C# 7.2

        // 参照に類する型の扱いが出来るように
        // ぶっちゃけ、Span<T> 構造体のために導入されたもの
        // Span<T> は中身に「ref フィールド」的なものを持っているので、ref と同じ扱いをしないといけない
        // 「Span<T> を持つ構造体」も、再帰的に同様の扱いにする必要あり

        // 引数由来の参照を戻り値に返すのはOK
        static Span<byte> ValidSpan(Span<byte> x) => x.Slice(1);

#if InvalidCode
        // ローカル由来の参照を戻り値に返すのはNG
        static Span<byte> InvalidSpan()
        {
            Span<byte> x = stackalloc byte[10];
            return x;
        }
#endif
    }
}
